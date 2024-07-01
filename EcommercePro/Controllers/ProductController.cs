using EcommercePro.DTO;
using EcommercePro.Models;
using EcommercePro.Repositiories;
using EcommercePro.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        private readonly IBrand _brandService;

        public ProductController(IProductRepository productRepository,
                                 IWebHostEnvironment environment,
                                 IFileService fileService,
                                 IBrand brandService)
        {
            _productRepository = productRepository;
            _environment = environment;
            _fileService = fileService;
            _brandService = brandService;
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
            }).ToList();
            return Products;
        }

        //[HttpGet("{id}")]
        //public ActionResult<ProductData> GetProductById(int id)
        //{
        //    Product product = _productRepository.Get(id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    ProductData productData = new ProductData()
        //    {
        //        Id = product.Id,
        //        Name = product.Name,
        //        Description = product.Description,
        //        Price = product.Price,
        //        Quentity = product.Quentity,
        //        CategoryId = product.CategoryId,

        //    };

        //    return productData;
        //}

        //[HttpGet("search/byname")]
        //public ActionResult<List<ProductData>> GetProductByName(string name)
        //{
        //    List<ProductData> products = _productRepository.GetProductByName(name).Select(product => new ProductData()
        //    {
        //        Id = product.Id,
        //        Name = product.Name,
        //        Description = product.Description,
        //        Price = product.Price,
        //        Quentity = product.Quentity,
        //        CategoryId = product.CategoryId,

        //    }).ToList();


        //    return products;
        //}


        //[HttpGet("search/byprice")]
        //public ActionResult<List<ProductData>> GetProductByPrice(decimal minPrice, decimal maxPrice)
        //{
        //    List<ProductData> products = _productRepository.GetProductByPriceRange(minPrice, maxPrice).Select(product => new ProductData()
        //    {
        //        Id = product.Id,
        //        Name = product.Name,
        //        Description = product.Description,
        //        Price = product.Price,
        //        Quentity = product.Quentity,
        //        CategoryId = product.CategoryId,

        //    }).ToList();

        //    return products;
        //}


        //[HttpGet("search/bycategory")]
        //public ActionResult<List<ProductData>> GetProductByCategory(int categoryId)
        //{
        //    List<ProductData> products = _productRepository.GetProductByCategory(categoryId).Select(product => new ProductData()
        //    {
        //        Id = product.Id,
        //        Name = product.Name,
        //        Description = product.Description,
        //        Price = product.Price,
        //        Quentity = product.Quentity,
        //        CategoryId = product.CategoryId,

        //    }).ToList();

        //    return products;
        //}

        //[HttpGet("search/bybrand")]
        //public ActionResult<List<ProductData>> GetProductByBrand(int brandId)
        //{
        //    List<ProductData> products = _productRepository.GetProductByBrand(brandId).Select(product => new ProductData()
        //    {
        //        Id = product.Id,
        //        Name = product.Name,
        //        Description = product.Description,
        //        Price = product.Price,
        //        Quentity = product.Quentity,
        //        CategoryId = product.CategoryId,
        //        //BrandId = product.BrandId
        //    }).ToList();


        //    return products;
        //}


        [HttpPost]
        [Authorize(Roles = "brand")]
        public async Task<IActionResult> PostProduct([FromForm] ProductData newProduct)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string userid = User.FindFirst("Id").Value;

                    if (userid != null)
                    {
                        int brandId = this._brandService.getByUSersID(userid).Id;

                        Product newproductData = new Product()
                        {
                            Name = newProduct.Name,
                            Description = newProduct.Description,
                            Price = newProduct.Price,
                            Quentity = newProduct.Quentity,
                            CategoryId = newProduct.CategoryId,
                            BrandId = brandId,
                            Discount = newProduct.Discount,
                            CreatedDate = DateOnly.FromDateTime(DateTime.Now)
                        };

                        _productRepository.Add(newproductData);

                        if (newProduct.formFiles != null && newProduct.formFiles.Count > 0)
                        {
                            foreach (var file in newProduct.formFiles)
                            {
                                var fileResult = _fileService.SaveImage(file);
                                if (fileResult.Item1 == 1)
                                {

                                    this._fileService.SaveImagesToDB(newproductData.Id, fileResult.Item2);
                                }
                            }
                            this._fileService.SaveChanges();
                        }

                        return Ok();

                    }

                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest("Cannot Add Product!!");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "brand")]
        public async Task<IActionResult> Update(int id, [FromForm] ProductData updateProduct)
        {
            if (ModelState.IsValid)
            {
                Product product = _productRepository.Get(id);

                string userid = User.FindFirst("Id").Value;
                if (userid != null)
                {
                    int brandId = this._brandService.getByUSersID(userid).Id;

                    bool isupdated = _productRepository.Update(id, new Product()
                    {
                        Id = id,
                        Name = updateProduct.Name,
                        Description = updateProduct.Description,
                        Price = updateProduct.Price,
                        Quentity = updateProduct.Quentity,
                        CategoryId = updateProduct.CategoryId,
                        Discount = updateProduct.Discount,
                        BrandId = brandId,
                        CreatedDate = DateOnly.FromDateTime(DateTime.Now)

                    });
                    if (isupdated)
                    {
                        if (updateProduct.formFiles != null && updateProduct.formFiles.Count > 0)
                        {
                            foreach (var file in updateProduct.formFiles)
                            {
                                var fileResult = _fileService.SaveImage(file);
                                if (fileResult.Item1 == 1)
                                {

                                    this._fileService.SaveImagesToDB(id, fileResult.Item2);
                                }
                            }
                            List<ProductImages> images = this._fileService.GetAll(id);
                            foreach(var image in images)
                            {
                                this._fileService.DeleteImage(image.imagePath);
                            }

                            this._fileService.DeleteImagesFromDB(id);

                            this._fileService.SaveChanges();
                        }
                        return Ok();
                    }
                }
            }
            return BadRequest("The Product Not Updated!!");
        }


        [HttpDelete("{id}")]
       [Authorize(Roles = "brand , admin")]
        public IActionResult Delete(int id)
        {
            bool isdeleted = _productRepository.Delete(id);
            if (isdeleted)
            {
                return Ok();
                 
            }
            return BadRequest("The Product Not Deleted");
        }

        //[HttpGet("ProductPagined")]
        //public ActionResult<Result> ProductPagined(int page = 1, int pageSize = 9)
        //{

            //    return this._productRepository.ProductPagined(page, pageSize);


            //}
            ////[HttpGet("prouctPaginedByBrand")]
            //public ActionResult<Result> ProductPaginedByBrand(int brandId, int page = 1, int pageSize = 9)
            //{
            //   Result Result  = this._productRepository.ProductPaginedByBrand(brandId, page, pageSize);

            //    return Result;



            //}





        }
}
