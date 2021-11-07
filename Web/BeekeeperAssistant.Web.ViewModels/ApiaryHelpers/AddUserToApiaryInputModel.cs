namespace BeekeeperAssistant.Web.ViewModels.ApiaryHelpers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public class AddUserToApiaryInputModel : IValidatableObject
    {
        public int ApiaryId { get; set; }

        public string ApiaryNumber { get; set; }

        [Required(ErrorMessage = "Полето 'Потребителско име' е задължително!")]
        [Display(Name = "Потребителско име")]
        public string UserName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var userManager = validationContext.GetService<UserManager<ApplicationUser>>();

            var errorList = new List<ValidationResult>();

            var user = userManager
                .FindByNameAsync(this.UserName)
                .GetAwaiter()
                .GetResult();

            if (user == null)
            {
                errorList.Add(new ValidationResult("Не съществува такъв потребител!", new List<string> { "UserName" }));
            }

            return errorList;
        }
    }
}
