namespace BeekeeperAssistant.Web.ViewModels.Administration.SystemNotification
{
    using System.ComponentModel.DataAnnotations;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class CreateNotificationInputModel : IMapFrom<SystemNotification>
    {
        [Required]
        [Display(Name = "Заглавие")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Съдържание")]
        public string Content { get; set; }

        [Display(Name = "Версия")]
        public string Version { get; set; }

        [Display(Name = "Линк за ресурс")]
        public string ImageUrl { get; set; }
    }
}
