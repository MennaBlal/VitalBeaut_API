using Azure;
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
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
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
        UserManager<ApplicationUser> _userManager;
        RoleManager<IdentityRole> _roleManager;
        private readonly IFileService _fileService;
        private IBrand _genaricBrandService;
        IHubContext<NotificationHub> _hubContext;
        IHttpContextAccessor _httpContextAccessor;
        IEmailService _emailService;
        IAuthService _authService;
        IUrlHelper _urlHelper;



        public UserController(Context _dbContext,UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            IFileService fileService ,IBrand genaricBrandService,IHubContext<NotificationHub> hubContext,IHttpContextAccessor httpContextAccessor,
            IEmailService emailService,IAuthService authService ,IUrlHelper urlHelper )
        {
            _userManager = userManager;
             _roleManager = roleManager;
            _fileService = fileService;
            _genaricBrandService = genaricBrandService;
            _hubContext = hubContext;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _authService = authService;
            dbContext = _dbContext;
            _urlHelper = urlHelper;
        }

        [HttpPost("Resigter")]
        public async Task<IActionResult> Register(UserRegister user)
        {

           if (ModelState.IsValid)
             {
                    var trans = await this.dbContext.Database.BeginTransactionAsync();
                    try
                    {
                        var Result = this._authService.CreateAcount(user);
                        if (Result.Result.status != 200)
                        {

                            return BadRequest(Result.Result.Errors);

                        }
                        ApplicationUser newuser = Result.Result.data as ApplicationUser;
                        //confirm email 

                        var code = await this._userManager.GenerateEmailConfirmationTokenAsync(newuser);
                        var requestAccessor = this._httpContextAccessor.HttpContext.Request;
                        var url = requestAccessor.Scheme + "://" + requestAccessor.Host + this._urlHelper.Action("ConfirmEmail", "User", new { userId = newuser.Id, code = code });
                        //$"/api/User/ConfirmEmail?userId={newuser.Id}&code={code}";
                        //email body
                         await this._emailService.SendEmail(newuser.Email, url);

                    await trans.CommitAsync();

                    return Ok();
                    }
                    catch (Exception ex)
                    {
                        await trans.RollbackAsync();

                        return BadRequest(ex.Message);


                    }

            }
            return BadRequest("Try again!!");  
           
        }
       
          [HttpGet("ConfirmEmail")]
          public async Task<IActionResult>ConfirmEmail([FromQuery] ConfirmEmailQuery emailQuery)
        {
            if (ModelState.IsValid) { 
    
                string Result = await this._authService.ConfirmEmail(emailQuery.userId, emailQuery.code);
                if (Result == "Success")
                    return Ok("The Email has been confirmed successfully. You can log in to the website");
                return BadRequest("Faild to Confirm Your Email");
            }
            return BadRequest("Faild to Confirm Your Email");

        }
    


        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLogin loginInfo)
        {
            ApplicationUser? Dbuser = await this._userManager.FindByNameAsync(loginInfo.username);
            if (Dbuser != null && Dbuser.IsDisable== false)
            {
                Brand branddb = this._genaricBrandService.getByUSersID(Dbuser.Id);

                if(branddb != null && branddb.Status == "Rejected")
                {
                    return BadRequest("Your request has been rejected.You can contact the Admin if you have any questions");
                }
                if(branddb !=null && branddb.Status == "Pending")
                {
                    return BadRequest("Wait for your application to be Accepted");
                }


                bool found = await this._userManager.CheckPasswordAsync(Dbuser, loginInfo.password);

                if (found == true)
                {
                    //confirm Email 
                    if (!Dbuser.EmailConfirmed)
                    {
                        return BadRequest("You Should Confirm Your Email");
                    }

                    List<Claim> claims = new List<Claim>();

                    claims.Add(new Claim("Name", Dbuser.UserName));

                    claims.Add(new Claim("Id", Dbuser.Id));

                    var roles = await this._userManager.GetRolesAsync(Dbuser);
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
            var TotalCount = this._userManager.Users.Count();

            var TotalPages = (int)Math.Ceiling((decimal)TotalCount / pageSize); 

            List<UserData> userDatas = new List<UserData>();
              
          List< ApplicationUser> users = this._userManager.Users
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToList();

            foreach (ApplicationUser user in users)
            {
              var Roles = this._userManager.GetRolesAsync(user).Result;

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
                ApplicationUser user =await this._userManager.FindByIdAsync(id);  
                if(user != null)
                {
                    user.UserName = userData.UserName;
                    user.PhoneNumber = userData.Phone;
                    user.Email = userData.Email;
                    if(userData.password != null) {

                        string resetToken = await this._userManager.GeneratePasswordResetTokenAsync(user);

                        IdentityResult result1 = await this._userManager.ResetPasswordAsync(user, resetToken, userData.password);

                        if (result1.Succeeded)
                        {
                            return Ok();
                        }
                        else
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

                    IdentityResult result = await this._userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
 
                      
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
            ApplicationUser user = await this._userManager.FindByIdAsync(Id);

            if(user != null)
            {

                //if this user is brand delete it from brands table and then delete from users table
                Brand branddb = this._genaricBrandService.getByUSersID(user.Id);
                if(branddb != null)
                {
                    this._genaricBrandService.Delete(branddb.Id);
                }
 
               IdentityResult result =  await this._userManager.DeleteAsync(user);
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
            ApplicationUser Userdb = await this._userManager.FindByIdAsync(id);

            if (Userdb != null)
            {
               IList<string>  Roles  = await this._userManager.GetRolesAsync(Userdb);

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
            ApplicationUser userdb = await this._userManager.FindByIdAsync(userid);

            if (userdb != null)
            {
                userdb.IsDisable = status;
               IdentityResult result =  await this._userManager.UpdateAsync(userdb);
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
