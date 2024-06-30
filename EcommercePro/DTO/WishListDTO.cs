using System.ComponentModel.DataAnnotations;

namespace EcommercePro.DTO
{
    public class WishListDTO
    {
        [Required]
        public string userId { get; set; }
        [Required]
        public int productId { get; set; }
    }
    public class ShowWishList
    {
        public int id { get; set; }
        public string productName { get; set; }
        public string productImage {  get; set; }  
        public decimal price { get; set; }
        public int productId { get; set; }

    }

}
