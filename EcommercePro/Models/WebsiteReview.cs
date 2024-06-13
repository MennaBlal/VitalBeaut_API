using System.ComponentModel.DataAnnotations.Schema;

namespace EcommercePro.Models
{
    public class WebsiteReview
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { set; get; }

        [ForeignKey("User")]
        public string UserId { set; get; }
        public ApplicationUser? User { get; set; }

        public DateOnly? CreatedDate { get; set; }

    }
}
