using EcommercePro.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommercePro.DTO
{
    public class UserRegister
    {
        [Required(ErrorMessage = "The UserName is Required")]
        public string username { set; get; }
        [Required(ErrorMessage = "The Password is Required")]
        public string password { set; get; }
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "The Email is Required")]
        public string email { set; get; }

        [Required(ErrorMessage = "The Role is Required")]

        public string Role { set; get; }

        /// <summary>
        /// if the regsiter is brand
        /// 
        /// </summary>
        
        [MinLength(12, ErrorMessage = "Enter The Invalid Tax Number")]
         [UniqueTax]
        public string? TaxNumber { set; get; } = "0123456789123";

         public string commercialRegistrationImage { set; get; } = "g.jpg";

        [NotMapped]
        public IFormFile? formFile2 { set; get; }

  
    }
}
