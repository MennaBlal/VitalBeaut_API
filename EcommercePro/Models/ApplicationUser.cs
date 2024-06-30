using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommercePro.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Image { set; get; }
        [NotMapped]
        public IFormFile? formFile { set; get; }
        public bool? IsDisable { set; get; } = false;
    }
}
