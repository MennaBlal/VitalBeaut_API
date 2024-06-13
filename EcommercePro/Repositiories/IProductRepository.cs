using EcommercePro.Models;

namespace EcommercePro.Repositiories
{
    public interface IProductRepository :  IGenaricService<Product>
    {
        List<Product> GetProductByName(string name);
        List<Product> GetProductByPriceRange(decimal minPrice, decimal maxPrice);
        List<Product> GetProductByCategory(int categoryId);
        List<Product> GetProductByBrand(int brandId);
    
    }
}
