using BeekeeperAssistant.Data.Common.Repositories;
using BeekeeperAssistant.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BeekeeperAssistant.Web.Infrastructure.ValidationAttributes.Beehives
{
    public class BeehiveNumberExistsValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var a = validationContext.Items;

            var apiaryRepository = validationContext.GetService<IDeletableEntityRepository<Beehive>>();
            var httpContext = validationContext.GetService<IHttpContextAccessor>();
            var userManager = validationContext.GetService<UserManager<ApplicationUser>>();

            return ValidationResult.Success;
        }
    }
}
