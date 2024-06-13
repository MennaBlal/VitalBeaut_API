using EcommercePro.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EcommercePro.DTO
{
    public class BrandDisplayData
    {
         public int Id { get; set; }
         public string BrandName { set; get; }
         public string email { set; get; }
        public string? phonenumber1 { set; get; }
        public string? phonenumber2 { set; get; }   
        public string TaxNumber { set; get; }
        public string Address { set; get; }
        public string  logoImage { set; get; }
        public string userId { set; get; }
        public string commercialRegistrationImage { set; get; }

        
    }
}
