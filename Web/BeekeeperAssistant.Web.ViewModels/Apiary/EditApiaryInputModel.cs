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
        public string CityCode { get; set; }

        [Required]
        [MaxLength(GlobalConstants.MaxPartNumberLength)]
        [RegularExpression(GlobalConstants.ApiaryPartNumberRegex)]
        public string FarmNumber { get; set; }

        // TODO: Make logic for number edit
        [RegularExpression(GlobalConstants.ApiaryFullNumberRegex)]
        public string Number { get; set; }

        public string Name { get; set; }

        [Required(ErrorMessage = GlobalConstants.ApiaryTypeRequiredErrorMessage)]
        public ApiaryType ApiaryType { get; set; }

        public string Adress { get; set; }
    }
}
