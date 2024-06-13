using EcommercePro.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommercePro.Repositiories
{
    public class WebsiteReviewRepo:IWebsiteReview
    {
        private readonly Context _dbContext;


        public WebsiteReviewRepo(Context context)
        {
            _dbContext = context;
        }
        public List<WebsiteReview> GetAll()
        {
            return _dbContext.WebsiteReviews.Include(review=>review.User).ToList();
        }

        public void Insert(WebsiteReview WebsiteReview)
        {
            if (WebsiteReview == null)
            {
                throw new ArgumentNullException(nameof(WebsiteReview));
            }

            _dbContext.Add(WebsiteReview);

        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

    }



}

