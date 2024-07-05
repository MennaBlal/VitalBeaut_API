using EcommercePro.DTO;
using EcommercePro.Models;
using EcommercePro.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EcommercePro.Repositiories
{
    public class ProductRepository : GenericRepo<Product>, IProductRepository
    {

        private readonly Context _context;

        public ProductRepository(Context context) : base(context)
        {
            _context = context;
        }

        public List<Product> GetProductByName(string name)
        {
           
           return  _context.Products.Where(p => p.Name.Contains(name) && p.IsDeleted == false).ToList();
        }

        public List<Product> GetProductByPriceRange(decimal minPrice, decimal maxPrice)
        {
            return _context.Products.Where(p => p.Price >= minPrice && p.Price <= maxPrice && p.IsDeleted == false).ToList();
        }

        public List<Product> GetProductByCategory(int categoryId)
        {
            return _context.Products.Where(p => p.CategoryId == categoryId && p.IsDeleted == false).ToList();
        }

        public List<Product> GetProductByBrand(int brandId)
        {
            return _context.Products.Where(p => p.BrandId == brandId && p.IsDeleted == false).ToList();
        }


        public Result ProductPaginedByBrand(int brandId, int pageIndex = 1, int pageSize = 9)
        {
            Result Result = new Result();

            var TotalCount = this._context.Products.Where(p => p.IsDeleted == false).Count();
            var TotalPages = (int)Math.Ceiling((decimal)TotalCount / pageSize);
            List<ProductDetails> Products = _context.Products.
                Where(p => p.Quentity > 0 && p.BrandId == brandId && p.IsDeleted == false)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p=>p.Images)
                .Select(p => new ProductDetails()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    images = (p.Images.FirstOrDefault()!=null)? p.Images.FirstOrDefault().imagePath:null,
                    Quentity = p.Quentity,
                    CategoryName = p.Category.Name,
                    BrandName = p.Brand.User.UserName

                })
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            Result.totalPages = TotalPages;
            Result.data = Products;
            Result.CurrentPage = pageIndex;
            return Result;

        }

        public Result ProductPagined(int pageIndex=1, int pageSize=9)
        {
            Result Result = new Result();

            var TotalCount = this._context.Products.Where(p=>p.IsDeleted==false).Count();
            var TotalPages = (int)Math.Ceiling((decimal)TotalCount / pageSize);
            List<ProductDetails> Products = _context.Products.
                Where(p => p.Quentity > 0  && p.IsDeleted == false )
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Images)
                .Select(p => new ProductDetails()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    images = (p.Images.FirstOrDefault() != null) ? p.Images.FirstOrDefault().imagePath : null,
                    Quentity = p.Quentity,
                    CategoryName = p.Category.Name,
                    BrandName = p.Brand.User.UserName

                })
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            Result.totalPages = TotalPages;
            Result.data = Products;
            Result.CurrentPage = pageIndex;
            return Result;
        }
    }
}
