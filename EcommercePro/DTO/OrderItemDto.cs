using EcommercePro.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommercePro.DTO
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
