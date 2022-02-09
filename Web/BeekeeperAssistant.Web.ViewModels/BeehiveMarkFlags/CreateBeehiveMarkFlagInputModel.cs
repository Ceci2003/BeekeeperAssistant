namespace BeekeeperAssistant.Web.ViewModels.BeehiveMarkFlags
{
    using System.ComponentModel.DataAnnotations;

    using BeekeeperAssistant.Data.Models;

    public class CreateBeehiveMarkFlagInputModel
    {
        [Display(Name = "Съдържание")]
        [Required(ErrorMessage = "Съдържанието е задължително.")]
        [MaxLength(500, ErrorMessage = "Съдържанието не може да бъде повече от 500 символа.")]
        [MinLength(5, ErrorMessage = "Съдържанието не може да бъде по-малко от 5 символа.")]
        public string Content { get; set; }

        [Display(Name = "Флаг")]
        public MarkFlagType FlagType { get; set; }

        public int BeehiveId { get; set; }

        public int BeehiveNumber { get; set; }

        public int BeehiveApiaryId { get; set; }

        public string BeehiveApiaryNumber { get; set; }

        public string BeehiveApiaryName { get; set; }
    }
}
