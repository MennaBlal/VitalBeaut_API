using EcommercePro.Models;

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
            return _dbContext.Set<WebsiteReview>().ToList();
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

