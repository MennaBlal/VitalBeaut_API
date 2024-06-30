using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security;

namespace EcommercePro.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string? phonenumber2 { set; get; }
         public string? Address { set; get; }
        
        public string TaxNumber { set; get; }

         public string commercialRegistrationImage { set; get; }
        public string Status { set; get; }

        [NotMapped]
        public IFormFile formFile2 { set; get; }

        [ForeignKey("User")]
        public string UserId { set; get; }
        public ApplicationUser? User { get; set; }


    }
}
