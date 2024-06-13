using System.ComponentModel.DataAnnotations;

namespace EcommercePro.Models
{
    public class UniqueTaxAttribute:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return null;
            string TaxNumber = value.ToString();
            Context context = validationContext.GetService<Context>();
            Brand Branddb = context.Brands.FirstOrDefault(Brand => Brand.TaxNumber == TaxNumber);
            if (Branddb != null)
                return new ValidationResult("The Tax Number  Is Exists");
            return ValidationResult.Success;
        }
    }
}
