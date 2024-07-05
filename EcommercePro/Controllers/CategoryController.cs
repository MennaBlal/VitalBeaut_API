using EcommercePro.DTO;
using EcommercePro.Models;
using EcommercePro.Repositiories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EcommercePro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private IGenaricService<Category> _genaricService;
       public CategoryController(IGenaricService<Category> genaricService) {
                 this._genaricService = genaricService;
        }
        [HttpGet]
        public ActionResult<List<CategoryData>> GetAll()
        {
            List<Category> categories = this._genaricService.GetAll();

            List<CategoryData> Categories = categories.Select(Cat=>new CategoryData()
            {
                Id=Cat.Id,
                Name=Cat.Name,
                Description=Cat.Description
            }).ToList();

            return Categories;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Add(CategoryData newCategory)
        {
        if(ModelState.IsValid)
            {
                
                try
                {              
                    this._genaricService.Add(new Category()
                    {
                        Name = newCategory.Name,
                        Description= newCategory.Description
                    });
                    return Ok();
                    
                }
                catch (Exception ex)
                {
                  return BadRequest(ex.Message);
                }
            }  
                return BadRequest("Not Add The Category");
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        public IActionResult Update(int id, CategoryData updateCategory)
        {
            if (ModelState.IsValid)
            {
                var isupdated = this._genaricService.Update(id, new Category()
                {
                    Id = id,
                     Name = updateCategory.Name,
                    Description = updateCategory.Description
                });
                
                if (isupdated)
                {

                    return Ok();
                }

            }
            return BadRequest("The Category Not Updated");

        }

        [HttpDelete]
        //[Authorize(Roles = "admin")]
        public IActionResult Delete(int id)
        {
            bool isdeleted = this._genaricService.Delete(id);
            if (isdeleted)
            {
                return Ok();

            }
            return BadRequest("The Category Not Deleted");
        }
    }
}
