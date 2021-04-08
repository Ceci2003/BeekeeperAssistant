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
        [RegularExpression(GlobalConstants.ApiaryPartNumberRegex)]
        [Display(Name = "Номер на населеното място")]
        public string CityCode { get; set; }

        [Required]
        [MaxLength(GlobalConstants.MaxPartNumberLength)]
        [RegularExpression(GlobalConstants.ApiaryPartNumberRegex)]
        [Display(Name = "Номер на пчелина")]
        public string FarmNumber { get; set; }

        [RegularExpression(GlobalConstants.ApiaryFullNumberRegex)]
        public string Number { get; set; }

        [Display(Name = "Име на пчелина")]
        public string Name { get; set; }

        [Required(ErrorMessage = GlobalConstants.ApiaryTypeRequiredErrorMessage)]
        [Display(Name = "Вид на пчелина")]
        public ApiaryType ApiaryType { get; set; }

        [Display(Name = "Населено място")]
        public string Adress { get; set; }
    }
}
