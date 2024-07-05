using System.ComponentModel.DataAnnotations;

namespace EcommercePro.DTO
{
    public class ConfirmEmailQuery
    {
        [Required(ErrorMessage ="Enter the userId")]
        public string userId { get; set; }
        [Required(ErrorMessage = "Enter the code")]
        public string code { get; set; }
    }
}
