using EcommercePro.DTO;
using EcommercePro.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommercePro.Repositiories
{
    public class ProductReviewService : IproductReview
    {
        private Context _context;
        public ProductReviewService(Context context) {

        _context = context;

        }
        public void Add(ProductReviewDTO productReview)
        {
            this._context.ProductReviews.Add(new ProductReview()
            {
                Rating = productReview.Rating,
                Comment = productReview.Comment,
                UserId = productReview.UserId,
                productId = productReview.ProductId,
                CreatedDate=DateOnly.FromDateTime(DateTime.Now.Date),
            });
            this._context.SaveChanges();
            
        }

        public List<ShowProductReview> GetProductReview(int productId)
        {

            List<ShowProductReview> productReviews = this._context.ProductReviews
                .Where(R => R.productId == productId)
                .Include(R=>R.product)
                .Include(R=>R.User)
                .Select(R => new ShowProductReview()
                {
                    Rating = R.Rating,
                    Comment= R.Comment,
                    username=R.User.UserName,
                    userimage=R.User.Image,
                    date = R.CreatedDate.ToString()
                    
                }).ToList();

            return productReviews;


             
        }
    }
}
