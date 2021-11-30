namespace BeekeeperAssistant.Web.Infrastructure.ValidationAttributes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ValidEnumerationValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult(this.ErrorMessage);
            }

            var type = value.GetType();

            return type.IsEnum && Enum.IsDefined(type, value) ?
                ValidationResult.Success :
                new ValidationResult(this.ErrorMessage);
        }
    }
}
