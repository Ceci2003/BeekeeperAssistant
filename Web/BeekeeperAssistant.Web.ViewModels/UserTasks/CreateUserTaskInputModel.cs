namespace BeekeeperAssistant.Web.ViewModels.UserTasks
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CreateUserTaskInputModel
    {
        [Display(Name = "Заглавие")]
        [Required(ErrorMessage = "Заглавието е задължително.")]
        public string Title { get; set; }

        [Display(Name = "Съдържание")]
        public string Content { get; set; }

        public string Color { get; set; }

        [Display(Name = "Начало")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Край")]
        public DateTime? EndDate { get; set; }

        public bool DisplayCss { get; set; }
    }
}
