using EcommercePro.DTO;
using EcommercePro.Models;
using EcommercePro.Repositiories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;

namespace EcommercePro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrand _BrandRepository;
        private readonly IFileService fileService;
        private readonly UserManager<ApplicationUser> userManager;
        public BrandController(IBrand BrandRepository , IFileService fileService , UserManager<ApplicationUser> userManager)
        {
            this._BrandRepository = BrandRepository;
            this.fileService = fileService;
            this.userManager = userManager;
        }



        [HttpPut("{id}")]
        [Authorize(Roles = "brand")]
        public async Task<IActionResult> Update(int id ,[FromForm] SetBrandData updateData)
        {
            if (ModelState.IsValid)
            {
                if (updateData.formFile1 != null)
                {
                    var fileResult = fileService.SaveImage(updateData.formFile1);
                    if (fileResult.Item1 == 1)
                    {
                        updateData.logoImage = fileResult.Item2;
                    }
                }
                
                if (updateData.formFile2 != null)
                {
                    var fileResult = fileService.SaveImage(updateData.formFile2);
                    if (fileResult.Item1 == 1)
                    {
                        updateData.commercialRegistrationImage = fileResult.Item2;
                    }
                }
                Brand brand = this._BrandRepository.Get(id);
                string OldBrandImage = brand.User.Image;
                string OldCommercialRegistrationImage = brand.commercialRegistrationImage;
                if (updateData.formFile1 != null)
                {
                    if (OldBrandImage != null)
                    {
                        await fileService.DeleteImage(OldBrandImage);

                    }
                }
                else
                {
                    updateData.logoImage = OldBrandImage;

                }
                if (updateData.formFile2 != null)
                {
                    if (OldCommercialRegistrationImage != null)
                    {
                        await fileService.DeleteImage(OldCommercialRegistrationImage);

                    }
                }
                else
                {
                    updateData.commercialRegistrationImage = OldCommercialRegistrationImage;
                }

                bool isUpdate = this._BrandRepository.Update(id, new Brand()
                {
                  commercialRegistrationImage = updateData.commercialRegistrationImage,
                  TaxNumber = updateData.TaxNumber ,
                  Address = updateData.Address,
                  phonenumber2=updateData.phonenumber2,
                  UserId=brand.UserId
                  

                });

                ApplicationUser user =await this.userManager.FindByIdAsync(brand.UserId);

                user.UserName = updateData.BrandName;
                user.PhoneNumber = updateData.phonenumber1;
                user.Email = updateData.email;
                user.Image = updateData.logoImage;
                if (updateData.password != null)
                {

                    string resetToken = await this.userManager.GeneratePasswordResetTokenAsync(user);

                    IdentityResult result1 = await this.userManager.ResetPasswordAsync(user, resetToken, updateData.password);

                    if (result1.Succeeded)
                    {
                        return Ok();
                    }
                    else
                    {
                        return BadRequest(result1.Errors);
                    }


                }
                 
               IdentityResult result =  await this.userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(result.Errors);
                }

            }
            return BadRequest("The data Not Updated");


        }
        [HttpGet]
        public ActionResult<List<BrandDisplayData>> getBrands()
        {
            List<Brand> brands = this._BrandRepository.GetAll();

            List<BrandDisplayData> brandsData = brands.Select(brand=>new BrandDisplayData()
            {
                userId =brand.UserId ,
                BrandName = brand.User.UserName,
                logoImage = brand.User.Image,
                commercialRegistrationImage = brand.commercialRegistrationImage,
                Id = brand.Id,
                phonenumber1 = brand.User.PhoneNumber,
                phonenumber2 = brand.phonenumber2,
                TaxNumber = brand.TaxNumber,
                email = brand.User.Email,
                Address = brand.Address,
                status=brand.Status

            }).ToList();

            return  brandsData;
        }
        [HttpGet("getBrand/{id}")]
        public ActionResult<BrandDisplayData> getBrand(int id)
        {
            Brand brand = this._BrandRepository.Get( id);

            if(brand != null)
            {
                return new BrandDisplayData()
                {
                    userId = brand.UserId,
                    BrandName = brand.User.UserName,
                    logoImage = brand.User.Image,
                    commercialRegistrationImage = brand.commercialRegistrationImage,
                    Id = brand.Id,
                    phonenumber1 = brand.User.PhoneNumber,
                    phonenumber2 = brand.phonenumber2,
                    TaxNumber = brand.TaxNumber,
                    email = brand.User.Email,
                    Address = brand.Address,
                    status=brand.Status


                };
            }
            return BadRequest("Not found The brand");

           
        }
        [HttpGet("getBrandByUserId/{userId}")]
        public ActionResult<BrandDisplayData> getBrandByUserId(string userId)
        {
            Brand brand = this._BrandRepository.getByUSersID(userId);

            if (brand != null)
            {
                return new BrandDisplayData()
                {
                    userId = brand.UserId,
                    BrandName = brand.User.UserName,
                    logoImage = brand.User.Image,
                    commercialRegistrationImage = brand.commercialRegistrationImage,
                    Id = brand.Id,
                    phonenumber1 = brand.User.PhoneNumber,
                    phonenumber2 = brand.phonenumber2,
                    TaxNumber = brand.TaxNumber,
                    email = brand.User.Email,
                    Address = brand.Address,
                    status = brand.Status


                };
            }
            return BadRequest("Not found The brand");


        }

    }
}
