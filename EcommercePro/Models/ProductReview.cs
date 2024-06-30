using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommercePro.Models
{
    public class ProductReview
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter Rating to Product")]
        public int Rating { get; set; } = 0;
        public string? Comment { set; get; }

        [ForeignKey("product")]
        public int productId { set; get; }

        public Product? product { set; get; }

        [ForeignKey("User")]
        public string UserId { set; get; }
        public ApplicationUser? User { get; set; }

        public DateOnly? CreatedDate { get; set; }

    }
}
