using EcommercePro.Models;

namespace EcommercePro.Repositiories
{
    public interface IWebsiteReview
    {

        public List<WebsiteReview> GetAll();
        void Insert(WebsiteReview websiteReview);
         void Save();
        public bool Delete(int ReviewId);

    }
}
