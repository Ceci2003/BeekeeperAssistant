namespace BeekeeperAssistant.Web.ViewModels.Administration.Communication
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BeekeeperAssistant.Data.Models;
    using Microsoft.AspNetCore.Http;

    public class SendEmailInputModel : IValidatableObject
    {
        [Required(ErrorMessage = "Полето 'От (имейл)' е задължително!")]
        [Display(Name = "От (имейл)")]
        public string FromEmail { get; set; }

        [Required(ErrorMessage = "Полето 'От (име)' е задължително!")]
        [Display(Name = "От (име)")]
        public string FromName { get; set; }

        [Display(Name = "Изпрати до")]
        public SendEmailOptions SendOptions { get; set; }

        [Display(Name = "До (имейл)")]
        public string To { get; set; }

        [Display(Name = "До (за повече имена използвайте ';' )")]
        public string ToMultiple { get; set; }

        [Required(ErrorMessage = "Полето 'Заглавие' е задължително!")]
        [Display(Name = "Заглавие")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Полето 'Съдържание' е задължително!")]
        [Display(Name = "Съдържание")]
        public string HtmlContent { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errorList = new List<ValidationResult>();

            if (this.SendOptions == SendEmailOptions.ToOne && string.IsNullOrWhiteSpace(this.To))
            {
                errorList.Add(new ValidationResult("Полето 'До (имейл)' е задължително!"));
            }

            if (this.SendOptions == SendEmailOptions.ToMultiple && string.IsNullOrWhiteSpace(this.ToMultiple))
            {
                errorList.Add(new ValidationResult("Полето 'До (имейли)' е задължително!"));
            }

            if (string.IsNullOrWhiteSpace(this.HtmlContent))
            {
                errorList.Add(new ValidationResult("Полето 'Съдържание' е задължително!"));
            }

            return errorList;
        }
    }
}
