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
        private readonly IProductImagesRepository _productImagesRepository;
        private readonly IBrand _brandRepository;
        public ProductController(IProductRepository productRepository,
                                 IWebHostEnvironment environment,
                                 IFileService fileService,
                                 IBrand brandService,
                                 IProductImagesRepository productImagesRepository,
                                 IBrand brandRepository)
        {
            _productRepository = productRepository;
            _environment = environment;
            _fileService = fileService;
            _brandService = brandService;
            _productImagesRepository = productImagesRepository;
            _brandRepository = brandRepository;
        }

        [HttpGet]
        public ActionResult GetAllProducts(int pageNumber = 1, int pageSize = 9)
        {
            var products = _productRepository.GetAll();

            var pagedProducts = products
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var productDataList = pagedProducts.Select(product =>
            {
                // Retrieve the first image for the product
                var productImage = _productImagesRepository.GetByProductId(product.Id).FirstOrDefault();
                string imageUrl = productImage?.imagePath;

                return new ProductData
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Quentity = product.Quentity,
                    CategoryId = product.CategoryId,
                    ImageUrl = imageUrl // Set the ImageUrl property
                };
            }).ToList();

            return Ok(new
            {
                TotalCount = products.Where(p => p.IsDeleted == false).Count(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(products.Where(p => p.IsDeleted == false).Count() / (double)pageSize),
                Products = productDataList
            });
        }


        [HttpGet("getallproductswithimages")]
        public ActionResult<List<ProductData>> GetAllProductsWithImages(int page = 1, int pageSize = 5)
        {
            var products = _productRepository.GetAll();

            // Calculate skip amount based on pagination parameters
            int skipAmount = (page - 1) * pageSize;

            // Paginate the products
            var paginatedProducts = products
                .Skip(skipAmount)
                .Take(pageSize)
                .Select(product =>
                {
                    // Retrieve all images for the product
                    var productImages = _productImagesRepository.GetByProductId(product.Id);
                    var imageUrls = productImages.Select(img => img.imagePath).ToList();

                    return new ProductData
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        Quentity = product.Quentity,
                        CategoryId = product.CategoryId,
                        ImageUrls = imageUrls // Set the ImageUrls property
                    };
                }).ToList();

            return paginatedProducts;
        }






        [HttpGet("{id}")]
        public ActionResult<ProductData> GetProductById(int id)
        {
            var product = _productRepository.Get(id);
            if (product == null)
            {
                return NotFound();
            }

            // Retrieve all images for the product
            var productImages = _productImagesRepository.GetByProductId(id);

          
            // Construct the product data DTO
            var productData = new ProductData
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quentity = product.Quentity,
                CategoryId = product.CategoryId,
                ImageUrls = productImages.Select(image=>image.imagePath).ToList() // Set the paginated ImageUrls property
            };

            return productData;
        }

        [HttpGet("search/byname")]
        public ActionResult GetProductByName(string name, int pageNumber = 1, int pageSize = 9)
        {
            var products = _productRepository.GetProductByName(name);

            var pagedProducts = products
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var productDataList = pagedProducts.Select(product =>
            {
                // Retrieve the first image for the product
                var productImage = _productImagesRepository.GetByProductId(product.Id).FirstOrDefault();
                string imageUrl = productImage?.imagePath;

                return new ProductData
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Quentity = product.Quentity,
                    CategoryId = product.CategoryId,
                    ImageUrl = imageUrl // Set the ImageUrl property
                };
            }).ToList();

            return Ok(new
            {
                TotalCount = products.Where(p => p.IsDeleted == false).Count(),
                PageNumber = pageNumber,
                TotalPages = (int)Math.Ceiling(products.Where(p => p.IsDeleted == false).Count() / (double)pageSize),
                Products = productDataList
            });
        }



        [HttpGet("search/byprice")]
        public ActionResult<List<ProductData>> GetProductByPrice(decimal minPrice, decimal maxPrice)
        {
            var products = _productRepository.GetProductByPriceRange(minPrice, maxPrice);

            var productDataList = products.Select(product =>
            {
                // Retrieve the first image for the product
                var productImage = _productImagesRepository.GetByProductId(product.Id).FirstOrDefault();
                string imageUrl = productImage?.imagePath;

                return new ProductData
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Quentity = product.Quentity,
                    CategoryId = product.CategoryId,
                    ImageUrl = imageUrl
                };
            }).ToList();

            return productDataList;
        }


        //
        [HttpGet("search/bycategory")]
        public ActionResult GetProductByCategory(int categoryId, int pageNumber = 1, int pageSize = 9)
        {
            var products = _productRepository.GetProductByCategory(categoryId);

            var pagedProducts = products
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var productDataList = pagedProducts.Select(product =>
            {
                // Retrieve the first image for the product
                var productImage = _productImagesRepository.GetByProductId(product.Id).FirstOrDefault();
               
                    string imageUrl = productImage?.imagePath;

                
                return new ProductData
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Quentity = product.Quentity,
                    CategoryId = product.CategoryId,
                    ImageUrl = imageUrl // Set the ImageUrl property
                };
            }).ToList();

            return Ok(new
            {
                TotalCount = products.Where(p => p.IsDeleted == false).Count(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(products.Where(p => p.IsDeleted == false).Count() / (double)pageSize),
                Products = productDataList
            });
        }

        //
        [HttpGet("search/bybrand")]
        public ActionResult GetProductByBrand(int brandId, int pageNumber = 1, int pageSize = 9)
        {
            var products = _productRepository.GetProductByBrand(brandId);

            // Retrieve the brand's information
            var brand = _brandRepository.Get(brandId);


            var pagedProducts = products
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var productDataList = pagedProducts.Select(product =>
            {
                // Retrieve the first image for the product
                var productImage = _productImagesRepository.GetByProductId(product.Id).FirstOrDefault();
                string imageUrl = productImage?.imagePath;

                return new ProductData
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Quentity = product.Quentity,
                    CategoryId = product.CategoryId,
                    ImageUrl = imageUrl,
                };
            }).ToList();

            return Ok(new
            {
                TotalCount = products.Where(p => p.IsDeleted == false).Count(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(products.Where(p=>p.IsDeleted==false).Count() / (double)pageSize),
                Products = productDataList
            });
        }

        [HttpPost]
        [Authorize(Roles = "brand")]
        public async Task<IActionResult> PostProduct([FromForm] SetProduct newProduct)
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
        public async Task<IActionResult> Update(int id, [FromForm] SetProduct updateProduct)
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
                            foreach (var image in images)
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

        
        [HttpGet("prouctPaginedByBrand")]
        public ActionResult<Result> ProductPaginedByBrand(int brandId, int page = 1, int pageSize = 9)
        {
            Result Result = this._productRepository.ProductPaginedByBrand(brandId, page, pageSize);

            return Result;



        }
        [HttpGet("ProductsPagined")]
        public ActionResult<Result> ProductsPagined(int page = 1, int pageSize = 6)
        {
            Result Result = this._productRepository.ProductPagined( page, pageSize);

            return Result;



        }




    }
}
