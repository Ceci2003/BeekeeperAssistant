namespace BeekeeperAssistant.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum SendEmailOptions
    {
        [Display(Name = "Всички")]
        ToEveryone = 1,
        [Display(Name = "Всички потвърдени имейли")]
        ToAllConfirmedEmails = 2,
        [Display(Name = "Един")]
        ToOne = 3,
        [Display(Name = "Няколко")]
        ToMultiple = 4,
    }
}
