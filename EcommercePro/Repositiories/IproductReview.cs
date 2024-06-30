using EcommercePro.DTO;

namespace EcommercePro.Repositiories
{
    public interface IproductReview
    {
        public void Add(ProductReviewDTO productReview);
        public List<ShowProductReview>GetProductReview(int productId);

    }
}
