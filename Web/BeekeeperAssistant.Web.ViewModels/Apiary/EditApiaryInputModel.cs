namespace BeekeeperAssistant.Web.ViewModels.Apiaries
{
    using System.ComponentModel.DataAnnotations;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class EditApiaryInputModel : IMapFrom<Apiary>
    {
        [Required]
        [MaxLength(4)]
        [RegularExpression(@"\d{4}")]
        public string CityCode { get; set; }

        [Required]
        [MaxLength(4)]
        [RegularExpression(@"\d{4}")]
        public string FarmNumber { get; set; }

        public string Number { get; set; }

        public string Name { get; set; }

        [Required]
        public ApiaryType ApiaryType { get; set; }

        public string Adress { get; set; }

        public string CreatorId { get; set; }
    }
}
