using System.ComponentModel.DataAnnotations.Schema;

namespace EcommercePro.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quentity { get; set;}
        public string? ImagePath { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

       [ForeignKey("Brand")]
        public int BrandId { set; get; }
        public Brand? Brand { get; set; }
        public List<ProductReview>? Reviews { get; set; }
        public DateOnly? CreatedDate { get; set; }
        
    }
}
