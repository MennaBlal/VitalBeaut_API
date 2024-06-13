using EcommercePro.DTO;
using EcommercePro.Models;
using EcommercePro.Repositiories;
using EcommercePro.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommercePro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IFileService _fileService;

        public ProductController(IProductRepository productRepository,
                                 IWebHostEnvironment environment,
                                 IFileService fileService)
        {
            _productRepository = productRepository;
            _environment = environment;
            _fileService = fileService;
        }

        [HttpGet]
        public ActionResult<List<ProductData>> GetAllProducts()
        {
            List<Product> products = _productRepository.GetAll();
            List<ProductData> Products = products.Select(Pro => new ProductData()
            {
                Id = Pro.Id,
                Name = Pro.Name,
                Description = Pro.Description,
                Price = Pro.Price,
                Quentity = Pro.Quentity,
                CategoryId = Pro.CategoryId,
                image = Pro.ImagePath
            }).ToList();
            return Products;
        }

        [HttpGet("{id}")]
        public ActionResult<ProductData> GetProductById(int id)
        {
            Product product = _productRepository.Get(id);
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

        [HttpGet("search/byname")]
        public ActionResult<List<ProductData>> GetProductByName(string name)
        {
            List<ProductData> products = _productRepository.GetProductByName(name).Select(product=>new ProductData()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quentity = product.Quentity,
                CategoryId = product.CategoryId,
                BrandId=product.BrandId
            }).ToList();

           
            return products;
        }


        [HttpGet("search/byprice")]
        public ActionResult<List<ProductData>> GetProductByPrice(decimal minPrice, decimal maxPrice)
        {
            List<ProductData> products = _productRepository.GetProductByPriceRange(minPrice, maxPrice).Select(product => new ProductData()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quentity = product.Quentity,
                CategoryId = product.CategoryId,
                BrandId = product.BrandId
            }).ToList();

            return products;
        }


        [HttpGet("search/bycategory")]
        public ActionResult<List<ProductData>> GetProductByCategory(int categoryId)
        {
            List<ProductData> products = _productRepository.GetProductByCategory(categoryId).Select(product => new ProductData()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quentity = product.Quentity,
                CategoryId = product.CategoryId,
                BrandId = product.BrandId
            }).ToList();

            return products;
        }

        [HttpGet("search/bybrand")]
        public ActionResult<List<ProductData>> GetProductByBrand(int brandId)
        {
            List<ProductData> products = _productRepository.GetProductByBrand(brandId).Select(product => new ProductData()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quentity = product.Quentity,
                CategoryId = product.CategoryId,
                BrandId = product.BrandId
            }).ToList();


            return products;
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
                    _productRepository.Add(new Product()
                    {
                        Name = newProduct.Name,
                        Description = newProduct.Description,
                        Price = newProduct.Price,
                        ImagePath = newProduct.image,
                        Quentity = newProduct.Quentity,
                        CategoryId = newProduct.CategoryId,
                        BrandId = newProduct.BrandId
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] ProductData updateProduct)
        {
            if (ModelState.IsValid)
            {
                Product product = _productRepository.Get(id);
                string oldImage = product.ImagePath;
                if (updateProduct.formFile != null)
                {
                    var fileResult = _fileService.SaveImage(updateProduct.formFile);
                    if (fileResult.Item1 == 1)
                    {
                        updateProduct.image = fileResult.Item2;
                    }
                }
                if (updateProduct.formFile != null && oldImage != null)
                {
                    await _fileService.DeleteImage(oldImage);
                }

                var isupdated = _productRepository.Update(id, new Product()
                {
                    Id = id,
                    Name = updateProduct.Name,
                    Description = updateProduct.Description,
                    Price = updateProduct.Price,
                    Quentity = updateProduct.Quentity,
                    CategoryId = updateProduct.CategoryId,
                    ImagePath = updateProduct.image,
                    BrandId = updateProduct.BrandId
                });
                if (isupdated)
                {
                    return Ok();
                }
            }
            return BadRequest("The Product Not Updated!!");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool isdeleted = _productRepository.Delete(id);
            if (isdeleted)
            {
                return Ok();
            }
            return BadRequest("The Product Not Deleted");
        }




       


    }
}
