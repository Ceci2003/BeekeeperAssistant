namespace BeekeeperAssistant.Web.ViewModels.Apiaries
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services;
    using BeekeeperAssistant.Services.Mapping;
    using BeekeeperAssistant.Web.ViewModels.Beehives;

    public class ApiaryDataViewModel : IMapFrom<Apiary>
    {
        public string Id { get; set; }

        [Display(Name = "Номер")]
        public string Number { get; set; }

        [Display(Name = "Вид")]
        public ApiaryType ApiaryType { get; set; }

        public string CreatorId { get; set; }

        [Display(Name = "Адрес")]
        public string Adress { get; set; }

        [Display(Name = "Име")]
        public string Name { get; set; }

        public ForecastResult ForecastResult { get; set; }

        public IEnumerable<BeehiveViewModel> Beehives { get; set; }
    }
}
