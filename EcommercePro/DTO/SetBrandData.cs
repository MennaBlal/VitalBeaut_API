using EcommercePro.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommercePro.DTO
{
    public class SetBrandData
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The UserName is Required")]
        public string BrandName { set; get; }
        [Required(ErrorMessage = "The Password is Required")]
        public string password { set; get; }
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "The Email is Required")]
        public string email { set; get; }

         
        public string? phonenumber1 { set; get; }
        public string? phonenumber2 { set; get; }
        [Length(12,12, ErrorMessage = "Enter The valid Tax Number")]
        [UniqueTax]
        [Required(ErrorMessage ="Enter The Tax Number")]
        public string TaxNumber { set; get; }

        public string Address { set; get; }
        public string? logoImage { set; get; }

        [NotMapped]
        public IFormFile? formFile1 { set; get; }

         public string? commercialRegistrationImage { set; get; }

        [NotMapped]
        [Required(ErrorMessage = "Enter the copy of commercial Registration")]

        public IFormFile formFile2 { set; get; }

    }
}
