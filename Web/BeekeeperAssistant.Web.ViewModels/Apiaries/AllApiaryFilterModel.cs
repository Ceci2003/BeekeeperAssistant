namespace BeekeeperAssistant.Web.ViewModels.Apiaries
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class AllApiaryFilterModel : IMapFrom<Apiary>, IMapFrom<ApiaryDataModel>
    {
        [Display(Name = "Номер")]
        public string Number { get; set; }

        [Display(Name = "Име")]
        public string Name { get; set; }

        [Display(Name = "Местоположение")]
        public string Adress { get; set; }

        [Display(Name = "Вид")]
        public ApiaryType ApiaryType { get; set; }

        [Display(Name = "Бр. кошери")]
        public int BeehivesCount { get; set; }
    }
}
