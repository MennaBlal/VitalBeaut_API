using EcommercePro.Models;
using EcommercePro.Repositories;

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
            return _context.Products.Where(p => p.Name.Contains(name)).ToList();
        }

        public List<Product> GetProductByPriceRange(decimal minPrice, decimal maxPrice)
        {
            return _context.Products
                           .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                           .ToList();
        }

        public List<Product> GetProductByCategory(int categoryId)
        {
            return _context.Products.Where(p => p.CategoryId == categoryId).ToList();
        }

        public List<Product> GetProductByBrand(int brandId)
        {
            return _context.Products.Where(p => p.BrandId == brandId).ToList();
        }
        
     
    }
}
