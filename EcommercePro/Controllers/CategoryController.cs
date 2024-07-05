using EcommercePro.DTO;
using EcommercePro.Models;
using EcommercePro.Repositiories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProductMiniApi.Repository.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommercePro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IGenaricService<Category> _genaricService;
        private readonly IWebHostEnvironment _environment;
        private readonly IFileService _fileService;

        public CategoryController(IGenaricService<Category> genaricService,
                                  IWebHostEnvironment environment,
                                  IFileService fileService)
        {
            _genaricService = genaricService;
            _environment = environment;
            _fileService = fileService;
        }
        [HttpGet]
        public ActionResult<List<CategoryData>> GetAll()
        {
            List<Category> categories = _genaricService.GetAll();

            List<CategoryData> categoryDataList = categories.Select(cat => new CategoryData
            {
                Id = cat.Id,
                Name = cat.Name,
                Description = cat.Description,
                ImagePath = cat.imagepath
            }).ToList();

            return categoryDataList;
        }

        [HttpGet, Route("{id}")]
        public ActionResult<CategoryData> GetCategoryById(int id)
        {
            Category category = this._genaricService.Get(id);
            if (category == null)
            {
                return NotFound();
            }

            CategoryData categoryData = new CategoryData()
            {
                Id=category.Id,
                Name = category.Name,
                Description=category.Description,
                ImagePath=category.imagepath
            };

            return categoryData;
        }

        //[Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] CategoryData newCategory)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (newCategory.FormFile != null)
                    {
                        var fileResult = _fileService.SaveImage(newCategory.FormFile);
                        if (fileResult.Item1 == 1)
                        {
                            newCategory.ImagePath = fileResult.Item2;
                        }
                    }
                    _genaricService.Add(new Category
                    {
                        Name = newCategory.Name,
                        Description = newCategory.Description,
                        imagepath = newCategory.ImagePath
                    });

                    return Ok();
                }
                catch (Exception ex)
                {

                    return BadRequest(ex.InnerException?.Message ?? ex.Message);
                }
            }

            return BadRequest("Category could not be added");
        }
        //[Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] CategoryData updateCategory)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (updateCategory.FormFile != null)
                    {
                        var fileResult = _fileService.SaveImage(updateCategory.FormFile);
                        if (fileResult.Item1 == 1)
                        {
                            updateCategory.ImagePath = fileResult.Item2;
                        }
                    }

                    var isUpdated = _genaricService.Update(id, new Category
                    {
                        Id = id,
                        Name = updateCategory.Name,
                        Description = updateCategory.Description,
                        imagepath = updateCategory.ImagePath
                    });

                    if (isUpdated)
                    {
                        return Ok();
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.InnerException?.Message ?? ex.Message);
                }
            }

            return BadRequest("Category could not be updated");
        }
        //[Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                bool isDeleted = _genaricService.Delete(id);
                if (isDeleted)
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }

            return BadRequest("Category could not be deleted");
        }
    }
}


