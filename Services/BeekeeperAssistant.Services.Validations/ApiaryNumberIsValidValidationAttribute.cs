using BeekeeperAssistant.Data.Common.Repositories;
using BeekeeperAssistant.Data.Models;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace BeekeeperAssistant.Services.Validations
{
    public class ApiaryNumberIsValidValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var service = validationContext.GetService<IDeletableEntityRepository<Apiary>>();

            // TODO: Implement validations

            return base.IsValid(value, validationContext);
        }
    }
}
