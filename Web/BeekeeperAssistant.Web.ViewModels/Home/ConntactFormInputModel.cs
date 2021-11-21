namespace BeekeeperAssistant.Web.ViewModels.Home
{
    using System.ComponentModel.DataAnnotations;

    public class ConntactFormInputModel
    {
        [Required(ErrorMessage = "Полето 'Имейл' е задължително!")]
        [EmailAddress]
        [Display(Name = "Имейл")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Полето 'Тема' е задължително!")]
        [Display(Name = "Тема")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Полето 'Съдържание' е задължително!")]
        [Display(Name = "Съдържание")]
        public string Content { get; set; }
    }
}
