using EcommercePro.DTO;
using EcommercePro.Models;

namespace EcommercePro.Repositiories
{
    public interface IProductRepository :  IGenaricService<Product>
    {
        List<Product> GetProductByName(string name);
        List<Product> GetProductByPriceRange(decimal minPrice, decimal maxPrice);
        List<Product> GetProductByCategory(int categoryId);
        List<Product> GetProductByBrand(int brandId);

       Result ProductPagined(int pageIndex, int pageSize);
       Result ProductPaginedByBrand(int brandId ,int pageIndex, int pageSize);


    }
}
