using EcommercePro.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommercePro.DTO
{
    public class ProductData
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The Name of Product is Reqiured")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "The Price of Product is Reqiured")]
        [ProductPrice]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "The Quantity of Product is Reqiured")]
        public int Quentity { get; set; }
        public string? image {  get; set; }
        public IFormFile? formFile { get; set; }
        [Required(ErrorMessage = "The Category of Product is Reqiured")]
        public int CategoryId { get; set; }

         
       

    }
}
