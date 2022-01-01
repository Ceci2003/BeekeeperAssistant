namespace BeekeeperAssistant.Web.ViewModels.ApiaryNotes
{
    using System.ComponentModel.DataAnnotations;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class EditApiaryNoteInputModel : IMapFrom<ApiaryNote>
    {
        public int Id { get; set; }

        [Display(Name = "Заглавие")]
        [MaxLength(150, ErrorMessage = "Заглавието не може да бъде повече от 150 символа.")]
        public string Title { get; set; }

        [Display(Name = "Съдържание")]
        [Required(ErrorMessage = "Съдържанието е задължително.")]
        [MaxLength(500, ErrorMessage = "Съдържанието не може да бъде повече от 500 символа."), MinLength(5, ErrorMessage = "Съдържанието не може да бъде по-малко от 5 символа.")]
        public string Content { get; set; }

        [Display(Name = "Цвят")]
        public string Color { get; set; }

        public int ApiaryId { get; set; }

        public string ApiaryNumber { get; set; }

        public string ApiaryName { get; set; }
    }
}
