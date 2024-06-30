using System.ComponentModel.DataAnnotations.Schema;

namespace EcommercePro.DTO
{
    public class UserData
    {
        
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string?Image { set; get; }
        public string Phone { set; get; }
        public bool isDisable {  get; set; }

    }

    public class UpdateData()
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string? password { get; set; }
        public string? Image { set; get; }
        [NotMapped]
        public IFormFile? formFile { get; set; }
        public string Phone { set; get; }

    }
}
