using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommercePro.Models
{
    public class Order
    {
        public int Id { set; get; }
        [Required(ErrorMessage = "The Status is Reqiured")]
        public string Status { set; get; }//Inprocessing - completed

        [ForeignKey("User")]
        public string UserId { set; get; }
        public ApplicationUser? User { get; set; }

        [ForeignKey("Payment")]
        public int PaymentId { set; get; }
        public Payment? Payment { set; get; }

        public DateOnly? CreatedDate { get; set; }

        public List<OrderItem> OrderItems { get; set; }

    }
}
