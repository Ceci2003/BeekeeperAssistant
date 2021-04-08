namespace BeekeeperAssistant.Web.Infrastructure.ValidationAttributes.Apiaries
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public class ApiaryExistsValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var apiaryRepository = validationContext.GetService<IDeletableEntityRepository<Apiary>>();
            var httpContext = validationContext.GetService<IHttpContextAccessor>();
            var userManager = validationContext.GetService<UserManager<ApplicationUser>>();

            var currentUserId = userManager.GetUserId(httpContext.HttpContext.User);
            var apiaryNumber = value as string;

            var doesApiaryExist = apiaryRepository.All()
                .Where(a => a.Number == apiaryNumber && a.CreatorId == currentUserId)
                .FirstOrDefault();

            if (doesApiaryExist == null)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
