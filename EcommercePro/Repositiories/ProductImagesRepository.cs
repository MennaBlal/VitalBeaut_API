using EcommercePro.Models;

namespace EcommercePro.Repositiories
{
    public class ProductImagesRepository:IProductImagesRepository
    {
        private readonly Context _context;

        public ProductImagesRepository(Context context)
        {
            _context = context;
        }
        public List<ProductImages> GetByProductId(int productId)
        {
            return _context.ProductImages.Where(pi => pi.productId == productId).ToList();
        }
    }
}
