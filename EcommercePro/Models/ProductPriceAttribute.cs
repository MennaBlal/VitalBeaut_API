using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.Xml;

namespace EcommercePro.Models
{
    public class ProductPriceAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            decimal? price = (decimal?)value;

            if (price <= 0)
            {
                return new ValidationResult("The product price can not be less that 0!");
            }

            return ValidationResult.Success;
        }
    }
}
