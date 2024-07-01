using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommercePro.Models
{
    public class ProductImages
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("product")]
        public int productId { set; get; }
        public Product? product { set; get; }

        [Required]
        public string imagePath { get; set; }

    }
}
