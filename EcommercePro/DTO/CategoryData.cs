using EcommercePro.Models;
using System.ComponentModel.DataAnnotations;

namespace EcommercePro.DTO
{
    public class CategoryData
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The Name of Category is Reqiured")]
        [UniqueCategory]
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile? FormFile { get; set; }

        public string? ImagePath {  get; set; }

    }
}
