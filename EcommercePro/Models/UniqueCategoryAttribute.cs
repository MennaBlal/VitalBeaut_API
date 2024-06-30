using EcommercePro.DTO;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.Xml;

namespace EcommercePro.Models
{ 
    public class UniqueCategoryAttribute:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return null;
            string CategoryName = value.ToString();
            Context context = validationContext.GetService<Context>();

            CategoryData CurrentCategory = (CategoryData)validationContext.ObjectInstance;

            Category categorydb = context.Categories.FirstOrDefault(category=>category.Name ==  CategoryName);
            if (categorydb!=null && CurrentCategory.Id != categorydb.Id)

                return new ValidationResult("The Category Name Is Exists");
            return ValidationResult.Success;
        }
    }
}
