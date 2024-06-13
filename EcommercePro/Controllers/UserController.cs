using EcommercePro.DTO;
using EcommercePro.Models;
using EcommercePro.Repositiories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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


        public UserController(Context _dbContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IFileService fileService ,
            IBrand genaricBrandService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this._fileService = fileService;
            this._genaricBrandService = genaricBrandService;
        }

        [HttpPost("Resigter")]
        public async Task<IActionResult> Register(UserRegister user)
        {

            if (ModelState.IsValid)
            {

                ApplicationUser? Dbuser = await userManager.FindByEmailAsync(user.email);
                if (Dbuser != null)
                {
                    return BadRequest("The Email is Exists Sign in or Register Another Email ");
                }

            
                ApplicationUser user1 = new ApplicationUser()
                {
                    UserName = user.username,
                    Email = user.email,
                    PasswordHash = user.password,
                };

                IdentityResult result = await userManager.CreateAsync(user1, user.password);

                if (user.Role == "brand")
                {
                    if (user.formFile2 != null)
                    {
                        var fileResult = _fileService.SaveImage(user.formFile2);
                        if (fileResult.Item1 == 1)
                        {
                            user.commercialRegistrationImage = fileResult.Item2; // getting name of image
                        }
                    }

                    if (result.Succeeded)
                    {
                        Brand brand = new Brand()
                        {
                            TaxNumber = user.TaxNumber,
                            commercialRegistrationImage = user.commercialRegistrationImage,
                            UserId = user1.Id
                        };
                        this._genaricBrandService.Add(brand);

                    }


                }


                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user1, user.Role);

                    return Created();
                }
                else
                {
                    return BadRequest(result.Errors);
                }




            }
            else
            {
                return BadRequest(ModelState);

            }

        }
        
    
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLogin loginInfo)
        {
            ApplicationUser? Dbuser = await userManager.FindByNameAsync(loginInfo.username);
            if (Dbuser != null)
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
                var KeyStr = Encoding.UTF8.GetBytes("1s3r4e5g6h7j81s3r4e5g6h7j81s3r4e5g6h7j81s3r4e5g6h7j8");
                var Key = new SymmetricSecurityKey(KeyStr);
                SigningCredentials signingCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);


                //create Token
                JwtSecurityToken MyToken = new JwtSecurityToken(
                   issuer: "http://localhost:5116",
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
                    Image=Dbuser.Image,
                    Role = roles[0]

                };

                return Ok(
                    new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(MyToken),
                        expired = MyToken.ValidTo,
                        User=user
                        

                    });



            }
            return BadRequest();

        }
        [HttpGet]
        public ActionResult< List<UserData> > GetUsers()
        {
            List<UserData> userDatas = new List<UserData>();
              
          List< ApplicationUser> users = this.userManager.Users.ToList();
            foreach (ApplicationUser user in users)
            {
              var Roles = this.userManager.GetRolesAsync(user).Result;

                userDatas.Add(new UserData()
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    Role = Roles.FirstOrDefault(),
                    Phone=user.PhoneNumber

                });
       
            }
            return userDatas;


        }
        [HttpPut]
        public  async Task<IActionResult> Update(string id , UpdateData userData)
        {
            if (id != null)
            {
                ApplicationUser user =await this.userManager.FindByIdAsync(id);  
                if(user != null)
                {
                    user.UserName = userData.UserName;
                    user.PhoneNumber = userData.Phone;
                    user.Email = userData.Email;
                    user.PasswordHash = userData.password;
                   
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
                           await this.userManager.RemoveFromRoleAsync(user, oldRole);
                            
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
        [HttpDelete]
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
        [HttpGet("GetUSer")]
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
    }
}
