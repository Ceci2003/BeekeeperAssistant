namespace BeekeeperAssistant.Web.ViewModels.Apiaries
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class CreateApiaryInputModel
    {
        [Required]
        [MaxLength(4)]
        [RegularExpression(@"\d{4}")]
        public string CityCode { get; set; }

        [Required]
        [MaxLength(4)]
        [RegularExpression(@"\d{4}")]
        public string FarmNumber { get; set; }

        public string Number => $"{this.CityCode}-{this.FarmNumber}";

        public string Name { get; set; }

        [Required]
        public ApiaryType ApiaryType { get; set; }

        [Display(Name = "Населено място", ShortName = "Населено място")]
        public string Adress { get; set; }
    }
}
