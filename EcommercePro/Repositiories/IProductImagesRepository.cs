using EcommercePro.Models;

namespace EcommercePro.Repositiories
{
    public interface IProductImagesRepository
    {
        List<ProductImages> GetByProductId(int productId);

    }
}
