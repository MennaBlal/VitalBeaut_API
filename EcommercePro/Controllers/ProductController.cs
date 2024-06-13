using EcommercePro.DTO;
using EcommercePro.Models;
using EcommercePro.Repositiories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProductMiniApi.Repository.Implementation;
namespace EcommercePro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IGenaricService<Product> _genaricProductService;
        IWebHostEnvironment _environment;
        private readonly IFileService _fileService;
        public ProductController(IGenaricService<Product> genaricProductService,
            IWebHostEnvironment environment,
            IFileService fileService)
        {
            this._genaricProductService = genaricProductService;
            _environment = environment;
            _fileService = fileService;
        }
        [HttpGet]
       public ActionResult<List<ProductData>> GetAllProducts()
       {
            List<Product> products = this._genaricProductService.GetAll();

            List<ProductData> Products = products.Select(Pro => new ProductData()
            {
                Id = Pro.Id,
                Name = Pro.Name,
                Description = Pro.Description,
                Price=Pro.Price,
                Quentity= Pro.Quentity,
                CategoryId=Pro.CategoryId,
                image=Pro.ImagePath
               
            }).ToList();
            return Products;
       }

        [HttpGet,Route("{id}")]
        public ActionResult<ProductData> GetProductById(int id)
        {
            Product product = this._genaricProductService.Get(id);
            if (product == null)
            {
                return NotFound();
            }

            ProductData productData = new ProductData()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quentity = product.Quentity,
                CategoryId = product.CategoryId,
            };

            return productData;
        }



        [HttpPost]
        public async Task<IActionResult> PostProduct([FromForm] ProductData newProduct)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (newProduct.formFile != null)
                    {
                        var fileResult = _fileService.SaveImage(newProduct.formFile);
                        if (fileResult.Item1 == 1)
                        {
                            newProduct.image = fileResult.Item2; 
                        }
                    }
                    _genaricProductService.Add(new Product()
                    {
                        Name = newProduct.Name,
                        Description = newProduct.Description,
                        Price = newProduct.Price,
                        ImagePath = newProduct.image,
                        Quentity = newProduct.Quentity,
                        CategoryId = newProduct.CategoryId
                    });

                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest("Cannot Add Product!!");
        }

        [HttpPut]
        public  async  Task<IActionResult> Update(int id, ProductData updateProduct)
        {
            if (ModelState.IsValid)
            {
                Product product = this._genaricProductService.Get(id);
                string oldImage = product.ImagePath;
                if (updateProduct.formFile != null)
                {
                    var fileResult = _fileService.SaveImage(updateProduct.formFile);
                    if (fileResult.Item1 == 1)
                    {
                        updateProduct.image = fileResult.Item2;
                    }
                }
                if (updateProduct.formFile != null)
                {
                    if (oldImage != null)
                    {
                        await _fileService.DeleteImage(oldImage);

                    }
                }

                var isupdated = this._genaricProductService.Update(id, new Product()
                {
                    Id = id,
                    Name = updateProduct.Name,
                    Description = updateProduct.Description,
                    Price = updateProduct.Price,
                    Quentity = updateProduct.Quentity,
                    CategoryId = updateProduct.CategoryId,
                    ImagePath=updateProduct.image
                });
                if (isupdated)
                {
                    return Ok();
                }
            }
            return BadRequest("The Product Not Updated!!");
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            bool isdeleted = this._genaricProductService.Delete(id);
            if (isdeleted)
            {
                return Ok();

            }
            return BadRequest("The Product Not Deleted");
        }
    }
}

