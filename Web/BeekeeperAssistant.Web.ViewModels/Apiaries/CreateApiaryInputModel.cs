namespace BeekeeperAssistant.Web.ViewModels.Apiaries
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using BeekeeperAssistant.Web.Infrastructure.ValidationAttributes.Apiaries;

    public class CreateApiaryInputModel
    {
        [Required(ErrorMessage = GlobalConstants.CityCodeRequiredErrorMessage)]
        [MaxLength(GlobalConstants.MaxPartNumberLength)]
        [RegularExpression(GlobalConstants.ApiaryPartNumberRegex)]
        [Display(Name = "Номер на населеното място")]
        public string CityCode { get; set; }

        [Required(ErrorMessage = GlobalConstants.FarmNumberRequiredErrorMessage)]
        [MaxLength(GlobalConstants.MaxPartNumberLength)]
        [RegularExpression(GlobalConstants.ApiaryPartNumberRegex)]
        [Display(Name = "Номер на обект")]
        public string FarmNumber { get; set; }

        [ApiaryExistsValidation(ErrorMessage = GlobalConstants.ApiaryExistsErrorMessage)]
        public string Number => $"{this.CityCode}-{this.FarmNumber}";

        [Display(Name = "Име на пчелина")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Вид на пчелина")]
        public ApiaryType ApiaryType { get; set; }

        [Required(ErrorMessage = GlobalConstants.CityCodeRequiredErrorMessage)]
        [Display(Name = "Населено място")]
        public string Adress { get; set; }
    }
}
