using System.ComponentModel.DataAnnotations;

namespace EcommercePro.DTO
{
    public class SendEmailCommend
    {
        [Required(ErrorMessage ="Enter the Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter the Message")]

        public string Meassage { get; set; }
    }
}
