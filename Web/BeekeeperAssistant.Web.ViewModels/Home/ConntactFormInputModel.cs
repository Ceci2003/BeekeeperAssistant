namespace BeekeeperAssistant.Web.ViewModels.Home
{
    using System.ComponentModel.DataAnnotations;

    public class ConntactFormInputModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Имейл")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Тема")]
        public string Subject { get; set; }

        [Required]
        [Display(Name = "Съдържание")]
        public string Content { get; set; }
    }
}
