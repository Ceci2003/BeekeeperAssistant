namespace BeekeeperAssistant.Web.ViewModels.Harvests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class AllByBeehiveIdHarvestFilterModel : IMapFrom<Harvest>, IMapFrom<HarvestDataModel>
    {
        [Display(Name = "Дата")]
        public DateTime DateOfHarves { get; set; }

        [Display(Name = "Количество")]
        public double Quantity { get; set; }

        [Display(Name = "Продукт")]
        public HarvestProductType HarvestProductType { get; set; }

        [Display(Name = "Име")]
        public string HarvestName { get; set; }
    }
}
