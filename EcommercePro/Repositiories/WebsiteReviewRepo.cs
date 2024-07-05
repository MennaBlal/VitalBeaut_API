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

        public bool Delete(int ReviewId)
        {
          WebsiteReview Review =  this._dbContext.WebsiteReviews.FirstOrDefault(review => review.Id == ReviewId);
            if (Review != null)
            {
                this._dbContext.WebsiteReviews.Remove(Review);
                this.Save();
                return true;

            }
            return false;
            
        }

        public List<WebsiteReview> GetAll()
        {
            return _dbContext.WebsiteReviews.Include(review=>review.User).ToList();
        }

        public void Insert(WebsiteReview WebsiteReview)
        {
            if (WebsiteReview != null)
            {
                _dbContext.WebsiteReviews.Add(WebsiteReview);
            }


        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

    }



}

