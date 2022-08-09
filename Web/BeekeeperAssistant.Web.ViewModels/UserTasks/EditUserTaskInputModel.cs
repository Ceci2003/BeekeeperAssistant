namespace BeekeeperAssistant.Web.ViewModels.UserTasks
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class EditUserTaskInputModel : IMapFrom<UserTask>
    {
        public int Id { get; set; }

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
    }
}
