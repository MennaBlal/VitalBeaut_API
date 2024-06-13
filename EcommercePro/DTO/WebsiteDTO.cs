using System.ComponentModel.DataAnnotations;

namespace EcommercePro.DTO
{
    public class WebsiteDTO
    {

         public int Rating { get; set; }
        public string? Comment { set; get; }
         public string UserId { set; get; }
    }
}
