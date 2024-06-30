using EcommercePro.DTO;
using EcommercePro.Hubs;
using EcommercePro.Models;
using EcommercePro.Repositiories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using ProductMiniApi.Repository.Implementation;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using System.Text;

namespace EcommercePro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        
        private Context dbContext;
        UserManager<ApplicationUser> userManager;
        RoleManager<IdentityRole> roleManager;
        private readonly IFileService _fileService;
        private IBrand _genaricBrandService;
        IHubContext<NotificationHub> _hubContext;
        IHttpContextAccessor _httpContextAccessor;
        IEmailService _emailService;




        public UserController(Context _dbContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IFileService fileService ,
            IBrand genaricBrandService,
            IHubContext<NotificationHub> hubContext,
            IHttpContextAccessor httpContextAccessor,
            IEmailService emailService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this._fileService = fileService;
            this._genaricBrandService = genaricBrandService;
            this._hubContext = hubContext;
            _httpContextAccessor = httpContextAccessor;
            this._emailService = emailService;
        }

        [HttpPost("Resigter")]
        public async Task<IActionResult> Register(UserRegister user)
        {

            try
            {
                if (ModelState.IsValid)
                {

                    ApplicationUser? Dbuser = await userManager.FindByEmailAsync(user.email);
                    if (Dbuser != null)
                    {
                        return BadRequest("The Email is Exists Sign in or Register Another Email ");
                    }


                    ActionResult<FunResult> userResult = await AddAsUser(user);

                    if (userResult.Value.status == 200)
                    {
                        ApplicationUser userdb = userResult.Value.data;
 

                        if (user.Role == "brand")
                        {

                            //send notification to Admin
                            IList<ApplicationUser> Admin = await this.userManager.GetUsersInRoleAsync("admin");
                            if (Admin.Count > 0 )
                            {
                                await this._hubContext.Clients.User(Admin[0].Id).SendAsync("SendNotification", user);

                            }
                            await AddAsBrand(user, userdb.Id);


                        }

                        await userManager.AddToRoleAsync(userdb, user.Role);

                        ////confirm email 
                        //if (!userdb.EmailConfirmed)
                        //{
                        //    return BadRequest("Please Confirm Your Email");
                        //}
                        //var code = await this.userManager.GenerateEmailConfirmationTokenAsync(userdb);
                        //var requestAccessor = this._httpContextAccessor.HttpContext.Request;
                        //var url = requestAccessor.Scheme + "://" + requestAccessor.Host + $"/api/User/ConfirmEmail?userId={userdb.Id}&code={code}";
                        ////email body
                        //var Emailsend = await this._emailService.SendEmail(userdb.Email, url);



                        return Created();

                    }
                    else
                    {
                        return BadRequest(userResult.Value.Errors);
                    }


                }
            }



            catch (Exception ex)
            {
                return BadRequest(ex.Message);


            }
            return Created();


        }

        [HttpPost("AddAsBrand")]
        private async Task<IActionResult> AddAsBrand(UserRegister userData ,string userId)
        {
 
            if (userData.formFile2 != null)
            {
                var fileResult = _fileService.SaveImage(userData.formFile2);
                if (fileResult.Item1 == 1)
                {
                    userData.commercialRegistrationImage = fileResult.Item2;
                }
            }
            if(userData.commercialRegistrationImage !=null && userData.TaxNumber != null)
            {
                Brand brand = new Brand()
                {
                    TaxNumber = userData.TaxNumber,
                    commercialRegistrationImage = userData.commercialRegistrationImage,
                    UserId = userId,
                    Status = "pending"
                };

                this._genaricBrandService.Add(brand);



                return Ok();

            }

            return BadRequest("Can`t Added this brand");
            
    }

        [HttpPost("AddAsUser")]
        private async Task<ActionResult<FunResult>> AddAsUser(UserRegister userData)
        {
            ApplicationUser user1 = new ApplicationUser()
            {
                UserName = userData.username,
                Email = userData.email,
                PasswordHash = userData.password,
                Image = "Basic_Ui__28186_29.jpg"
            };


            IdentityResult result = await userManager.CreateAsync(user1, userData.password);

            if (result.Succeeded)
            {
 
                return new FunResult()
                {
                    status=200,
                    data= user1,
                    Errors=null

                };
            }
            else
            {
                return new FunResult()
                {
                    status = 400,
                    data = null,
                    Errors = (List<IdentityError>)result.Errors

                };
            }

           

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLogin loginInfo)
        {
            ApplicationUser? Dbuser = await userManager.FindByNameAsync(loginInfo.username);
            if (Dbuser != null && Dbuser.IsDisable== false)
            {
                Brand branddb = this._genaricBrandService.getByUSersID(Dbuser.Id);

                if(branddb != null && branddb.Status == "rejected")
                {
                    return BadRequest("Your request has been rejected.You can contact the admin if you have any questions");
                }
                if(branddb !=null && branddb.Status == "pending")
                {
                    return BadRequest("Wait for your application to be accepted");
                }
                bool found = await this.userManager.CheckPasswordAsync(Dbuser, loginInfo.password);
                if (found == true)
                {

                    List<Claim> claims = new List<Claim>();

                    claims.Add(new Claim("Name", Dbuser.UserName));

                    claims.Add(new Claim("Id", Dbuser.Id));

                    var roles = await userManager.GetRolesAsync(Dbuser);
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));

                    }

                    //key and algorithm
                    var KeyStr = Encoding.UTF8.GetBytes("1s3r4e5g6h7j81s3r4e5g6h7j81s3r4e5g6h7j81s3r4e5g6h7j89");
                    var Key = new SymmetricSecurityKey(KeyStr);
                    SigningCredentials signingCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);


                    //create Token
                    JwtSecurityToken MyToken = new JwtSecurityToken(
                       issuer: "http://localhost:5261",
                       audience: "http://localhost:4200",
                       expires: DateTime.Now.AddHours(30),
                       claims: claims,
                       signingCredentials: signingCredentials


                       );

                    UserData user = new UserData()
                    {
                        Id = Dbuser.Id,
                        UserName = Dbuser.UserName,
                        Email = Dbuser.Email,
                        Phone = Dbuser.PhoneNumber,
                        Image = Dbuser.Image,
                        Role = roles[0]

                    };

                    return Ok(
                        new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(MyToken),
                            expired = MyToken.ValidTo,
                            User = user


                        });


                }
            }
            return BadRequest("username or password Invaild");

        }

        [HttpGet("{page:int}")    ]
        [Authorize]
        public ActionResult< List<UserData> > GetUsers(int page = 1 , int pageSize = 7)
        {
            var TotalCount = this.userManager.Users.Count();

            var TotalPages = (int)Math.Ceiling((decimal)TotalCount / pageSize); 

            List<UserData> userDatas = new List<UserData>();
              
          List< ApplicationUser> users = this.userManager.Users
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToList();

            foreach (ApplicationUser user in users)
            {
              var Roles = this.userManager.GetRolesAsync(user).Result;

                userDatas.Add(new UserData()
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    Role = Roles.FirstOrDefault(),
                    Phone=user.PhoneNumber,
                    isDisable= (bool)user.IsDisable

                });
       
            }
            return Ok(
                new
                {
                    userDatas =userDatas,
                    count=TotalCount
                }
                );


        }
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(string id , UpdateData userData)
        {
            if (id != null)
            {
                ApplicationUser user =await this.userManager.FindByIdAsync(id);  
                if(user != null)
                {
                    user.UserName = userData.UserName;
                    user.PhoneNumber = userData.Phone;
                    user.Email = userData.Email;
                    if(userData.password != null) {
                        var token = await this.userManager.GeneratePasswordResetTokenAsync(user);

                        IdentityResult result1 =  await this.userManager.ChangePasswordAsync(user, token, userData.password);

                        if (!result1.Succeeded)
                        {
                            return BadRequest(result1.Errors);
                        }
 
                    }

                    string oldimage = user.Image;
                 
                    if (userData.formFile != null)
                    {
                        var fileResult = _fileService.SaveImage(userData.formFile);
                        if (fileResult.Item1 == 1)
                        {
                            user.Image = fileResult.Item2; // getting name of image
                        }
                    }
                    if (userData.formFile != null)
                    {
                        if(oldimage != null)
                        {
                            await _fileService.DeleteImage(oldimage);

                        }
                    }
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
 
                      IList<string>  roles= await this.userManager.GetRolesAsync(user);
                        string oldRole = roles.FirstOrDefault();

                        if(oldRole != userData.Role)
                        {
                           //await this.userManager.RemoveFromRoleAsync(user, oldRole);
                            
                           await this.userManager.AddToRoleAsync(user,userData.Role);

                         }
                    
                        return Ok();
                         

                    }




                }
                else
                {
                    return NotFound();
                }

            }
            return BadRequest("Not Updated");
        }

        [HttpDelete("{Id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string Id)
        {
            ApplicationUser user = await this.userManager.FindByIdAsync(Id);

            if(user != null)
            {

                //if this user is brand delete it from brands table and then delete from users table
                Brand branddb = this._genaricBrandService.getByUSersID(user.Id);
                if(branddb != null)
                {
                    this._genaricBrandService.Delete(branddb.Id);
                }
 
               IdentityResult result =  await this.userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return Ok();
                }

            }
             
                return NotFound();
            


        }
        [HttpGet("GetUSer/{id}")]
        public async Task<ActionResult<UserData>> GetUSer(string id)
        {
            ApplicationUser Userdb = await this.userManager.FindByIdAsync(id);

            if (Userdb != null)
            {
               IList<string>  Roles  = await this.userManager.GetRolesAsync(Userdb);

                UserData user = new UserData()
                {
                    Id = Userdb.Id,
                    UserName = Userdb.UserName,
                    Email = Userdb.Email,
                    Phone = Userdb.PhoneNumber,
                    Image = Userdb.Image,
                    Role = Roles.FirstOrDefault()

                };
                return user;
            }
            return NotFound();

        }


        [HttpGet("adminApprove/{brandId}/{status}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult>  adminApprove(int brandId, string status)
        {
            Brand brand = this._genaricBrandService.Get(brandId);
            if (brand != null)
            {
                
                 
                    brand.Status = status;
                 
                this._genaricBrandService.Save();

                return Created();

            }
            return BadRequest("Can`t updated the Status of brand");
                   

            

        }

        [HttpGet("change-status/{userid}/{status}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> ChangeStatus(string userid , bool status)
        {
            ApplicationUser userdb = await this.userManager.FindByIdAsync(userid);

            if (userdb != null)
            {
                userdb.IsDisable = status;
               IdentityResult result =  await this.userManager.UpdateAsync(userdb);
                if (result.Succeeded)
                {
                    return Ok();

                }
                return BadRequest("Faild to update status");


            }
            return NotFound("Not Found the user");

        }

    }
}
