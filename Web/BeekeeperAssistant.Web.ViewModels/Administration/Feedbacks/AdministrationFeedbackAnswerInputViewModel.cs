namespace BeekeeperAssistant.Web.ViewModels.Administration.Feedbacks
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class AdministrationFeedbackAnswerInputViewModel : IMapFrom<Feedback>
    {
        // Feedback data
        public int Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public DateTime CreatedOn { get; set; }

        public FeedbackType FeedbackType { get; set; }

        [Display(Name = "Получател - име")]
        public string UserUserName { get; set; }

        [Display(Name = "Получател - имейл")]
        public string UserEmail { get; set; }

        // Data for input form
        [Display(Name = "Автор - имейл")]
        public string SenderEmail { get; set; }

        [Display(Name = "Автор - име")]
        public string SenderName { get; set; }

        [Required(ErrorMessage = "Полето 'Заглавие' е задължително!")]
        [Display(Name = "Заглавие")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Полето 'Съдържание' е задължително!")]
        [Display(Name = "Съдържание")]
        public string AnswerContent { get; set; }
    }
}
