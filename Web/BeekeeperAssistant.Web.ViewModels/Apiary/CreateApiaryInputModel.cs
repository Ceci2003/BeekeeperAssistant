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
        public string CityCode { get; set; }

        [Required(ErrorMessage = GlobalConstants.FarmNumberRequiredErrorMessage)]
        [MaxLength(GlobalConstants.MaxPartNumberLength)]
        [RegularExpression(GlobalConstants.ApiaryPartNumberRegex)]
        public string FarmNumber { get; set; }

        [ApiaryExistsValidation(ErrorMessage = GlobalConstants.ApiaryExistsErrorMessage)]
        public string Number => $"{this.CityCode}-{this.FarmNumber}";

        public string Name { get; set; }

        [Required]
        public ApiaryType ApiaryType { get; set; }

        [Display(Name = "Населено място")]
        public string Adress { get; set; }
    }
}
