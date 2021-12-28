namespace BeekeeperAssistant.Web.ViewModels.Apiaries
{
    using System.ComponentModel.DataAnnotations;

    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class EditApiaryInputModel : IMapFrom<Apiary>
    {
        [Required]
        [MaxLength(GlobalConstants.MaxPartNumberLength)]
        [RegularExpression(GlobalConstants.ApiaryPartNumberRegex, ErrorMessage = "Пощенският код не съществува или не е валиден. Трябва да въведете 4 цифри.")]
        [Display(Name = "Пощенски код")]
        public string CityCode { get; set; }

        [Required]
        [MaxLength(GlobalConstants.MaxPartNumberLength)]
        [RegularExpression(GlobalConstants.ApiaryPartNumberRegex, ErrorMessage = "Невалиден номер на пчелин. Трябва да въведете 4 цифри.")]
        [Display(Name = "Номер на пчелина")]
        public string FarmNumber { get; set; }

        [RegularExpression(GlobalConstants.ApiaryFullNumberRegex)]
        public string Number { get; set; }

        [Display(Name = "Име на пчелина")]
        public string Name { get; set; }

        [Required(ErrorMessage = GlobalConstants.ApiaryTypeRequiredErrorMessage)]
        [Display(Name = "Вид на пчелина")]
        public ApiaryType ApiaryType { get; set; }

        [Display(Name = "Местоположение")]
        public string Adress { get; set; }
    }
}
