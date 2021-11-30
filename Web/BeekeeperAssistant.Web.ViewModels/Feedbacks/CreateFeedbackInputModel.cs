namespace BeekeeperAssistant.Web.ViewModels.Feedbacks
{
    using System.ComponentModel.DataAnnotations;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Web.Infrastructure.ValidationAttributes;
    using Microsoft.AspNetCore.Mvc;

    public class CreateFeedbackInputModel
    {
        [Required(ErrorMessage = "Полето 'Заглавие' е задължително!")]
        [Display(Name = "Заглавие")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Полето 'Вид отзив' е задължително!")]
        [ValidEnumerationValidation(ErrorMessage = "Невалиден 'Вид отзив'!")]
        [Display(Name = "Вид отзив")]
        public FeedbackType FeedbackType { get; set; }

        [Required(ErrorMessage = "Полето 'Описание' е задължително!")]
        [Display(Name = "Описание")]
        public string Body { get; set; }
    }
}
