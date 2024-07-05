using EcommercePro.DTO;
using EcommercePro.Hubs;
using EcommercePro.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.SignalR;
using System.Runtime.Intrinsics.X86;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EcommercePro.Repositiories
{
    public class AuthService : IAuthService
    {
        private Context dbContext;
        UserManager<ApplicationUser> userManager;
        RoleManager<IdentityRole> roleManager;
        private readonly IFileService _fileService;
        private IBrand _genaricBrandService;
        IHubContext<NotificationHub> _hubContext;
        IHttpContextAccessor _httpContextAccessor;
        IEmailService _emailService;
        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            IFileService fileService, IBrand genaricBrandService, IHubContext<NotificationHub> hubContext,
            IHttpContextAccessor httpContextAccessor, IEmailService emailService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this._fileService = fileService;
            this._genaricBrandService = genaricBrandService;
            this._hubContext = hubContext;
            _httpContextAccessor = httpContextAccessor;
            this._emailService = emailService;
        }

        public async Task<FunResult> CreateAcount(UserRegister user)
        {
            ApplicationUser? Dbuser = await userManager.FindByEmailAsync(user.email);
            if (Dbuser != null)
            {
                return new FunResult()
                {
                    status = 400,
                    data = null,
                    Errors = "The Email is Exists Sign in or Register Another Email"

                };
            }

            ActionResult<FunResult> userResult = await AddAsUser(user);

            if (userResult.Value.status == 200)
            {
                ApplicationUser userdb = userResult.Value.data;


                if (user.Role == "brand")
                {

                    //send notification to Admin
                    IList<ApplicationUser> Admin = await this.userManager.GetUsersInRoleAsync("admin");
                    if (Admin.Count > 0)
                    {
                        await this._hubContext.Clients.User(Admin[0].Id).SendAsync("SendNotification", user);

                    }

                   FunResult BrandResult =  AddAsBrand(user, userdb.Id);

                    if(BrandResult.status == 400)
                    {
                        return new FunResult()
                        {
                            status = 400,
                            data = null,
                            Errors = BrandResult.Errors

                        };
                    }


                }

                await userManager.AddToRoleAsync(userdb, user.Role);

                return new FunResult()
                {
                    status = 200,
                    data = userdb,
                    Errors = null

                };

            }
            else
            {
                return new FunResult()
                {
                    status = 400,
                    data =null,
                    Errors = userResult.Value.Errors

                }; 
            }
        }

        public FunResult AddAsBrand(UserRegister userData, string userId)
        {

            try
            {
                if (userData.formFile2 != null)
                {
                    var fileResult = _fileService.SaveImage(userData.formFile2);
                    if (fileResult.Item1 == 1)
                    {
                        userData.commercialRegistrationImage = fileResult.Item2;
                    }
                    else
                    {
                        return new FunResult()
                        {
                            status = 400,
                            data = null,
                            Errors = fileResult.Item2

                        };
                     }
                }
                if (userData.commercialRegistrationImage != null && userData.TaxNumber != null)
                {
                    Brand brand = new Brand()
                    {
                        TaxNumber = userData.TaxNumber,
                        commercialRegistrationImage = userData.commercialRegistrationImage,
                        UserId = userId,
                        Status = "Pending"
                    };

                    this._genaricBrandService.Add(brand);



                    return new FunResult()
                    {
                        status = 200,
                        data = null,
                        Errors = null

                    };

                }

            }
            catch (Exception ex)
            {
                return new FunResult()
                {
                    status = 400,
                    data = null,
                    Errors = ex.Message

                };
             }
             return new FunResult()
            {
                status = 400,
                data = null,
                Errors = "Faild to Add This Brand"

            };


        }

        public async Task<FunResult> AddAsUser(UserRegister userData)
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
                    status = 200,
                    data = user1,
                    Errors = null

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
        public async Task<string>ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
                return "Faild";
            var user = await this.userManager.FindByIdAsync(userId);
            var ConfirmEmail = await this.userManager.ConfirmEmailAsync(user, code);
            if (!ConfirmEmail.Succeeded)
                return "Faild";
            return "Success";

        }
    }
}