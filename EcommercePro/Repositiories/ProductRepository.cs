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
           
           return  _context.Products.Where(p => p.Name.Contains(name)).ToList();
        }

        public List<Product> GetProductByPriceRange(decimal minPrice, decimal maxPrice)
        {
            return _context.Products.Where(p => p.Price >= minPrice && p.Price <= maxPrice).ToList();
        }

        public List<Product> GetProductByCategory(int categoryId)
        {
            return _context.Products.Where(p => p.CategoryId == categoryId).ToList();
        }

        public List<Product> GetProductByBrand(int brandId)
        {
            return _context.Products.Where(p => p.BrandId == brandId).ToList();
        }

        // public Result ProductPagined(int pageIndex = 1, int pageSize = 9)
        // {
        //     Result Result = new Result();

        //     var TotalCount = this._context.Products.Count();
        //     var TotalPages = (int)Math.Ceiling((decimal)TotalCount / pageSize);
        //     List <ProductShow>Products =  _context.Products.Where(p=>p.Quentity > 0).Include(p => p.Category).Include(p=>p.Brand)
        //         .OrderByDescending(p=>p.CreatedDate)
        //         .Select(p=>new ProductShow()
        //         {
        //             Id =p.Id,
        //             Name =p.Name,
        //             Price =p.Price,
        //             Description=p.Description,
        //             image=p.ImagePath,
        //             Quentity=p.Quentity,
        //             CategoryName=p.Category.Name,
        //             BrandName=p.Brand.User.UserName

        //         })
        //         .Skip((pageIndex - 1)*pageSize)
        //         .Take(pageSize)
        //         .ToList();

        //     Result.totalPages = TotalPages;
        //     Result.data = Products;
        //     Result.CurrentPage = pageIndex;
        //     return Result;

        // }
        //public Result ProductPaginedByBrand(int brandId, int pageIndex = 1, int pageSize = 9 )
        // {
        //     Result Result = new Result();

        //     var TotalCount = this._context.Products.Count();
        //     var TotalPages = (int)Math.Ceiling((decimal)TotalCount / pageSize);
        //     List<ProductShow> Products = _context.Products.
        //         Where(p => p.Quentity > 0 && p.BrandId == brandId)
        //         .Include(p => p.Category)
        //         .Include(p => p.Brand)
        //         .Select(p => new ProductShow()
        //         {
        //             Id = p.Id,
        //             Name = p.Name,
        //             Price = p.Price,
        //             Description = p.Description,
        //             image = p.ImagePath,
        //             Quentity = p.Quentity,
        //             CategoryName = p.Category.Name,
        //             BrandName = p.Brand.User.UserName

        //         })
        //         .Skip((pageIndex - 1) * pageSize)
        //         .Take(pageSize)
        //         .ToList();

        //     Result.totalPages = TotalPages;
        //     Result.data = Products;
        //     Result.CurrentPage = pageIndex;
        //     return Result;

        // }






    }
}
