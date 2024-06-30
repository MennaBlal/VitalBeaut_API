using EcommercePro.DTO;
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

            if (validationContext.ObjectType == typeof(SetBrandData))
            {
                SetBrandData CurrentBrand = (SetBrandData)validationContext.ObjectInstance;
                if (Branddb != null && CurrentBrand.Id!=Branddb.Id )
                    return new ValidationResult("The Tax Number  Is Exists");

            }
            else
            {
                UserRegister userRegister = (UserRegister)validationContext.ObjectInstance;
                if (Branddb != null )
                    return new ValidationResult("The Tax Number  Is Exists");

            }

            return ValidationResult.Success;
        }
    }
}
