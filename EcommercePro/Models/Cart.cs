using System.ComponentModel.DataAnnotations.Schema;

namespace EcommercePro.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("product")]
        public int productId { set; get; }

        public Product? product { set; get; }

        [ForeignKey("User")]
        public string UserId { set; get; }
        public ApplicationUser? User { get; set; }

        public DateOnly? CreatedDate { get; set; }

    }
}
