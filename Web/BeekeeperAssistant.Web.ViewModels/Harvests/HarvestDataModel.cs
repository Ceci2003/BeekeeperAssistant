namespace BeekeeperAssistant.Web.ViewModels.Harvests
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using BeekeeperAssistant.Data.Filters.Contracts;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class HarvestDataModel : IMapFrom<Harvest>, IMapFrom<HarvestDataModel>, IDefaultOrder<HarvestDataModel>
    {
        public int Id { get; set; }

        public string CreatorId { get; set; }

        [Display(Name = "Име")]
        public string HarvestName { get; set; }

        [Display(Name = "Дата на добива")]
        public DateTime DateOfHarves { get; set; }

        [Display(Name = "Бележка")]
        public string Note { get; set; }

        [Display(Name = "Добит продукт")]
        public HarvestProductType HarvestProductType { get; set; }

        [Display(Name = "Вид мед")]
        public HoneyType HoneyType { get; set; }

        [Display(Name = "Количество")]
        public double Quantity { get; set; }

        [Display(Name = "Еденица")]
        public Unit Unit { get; set; }

        public DateTime CreatedOn { get; set; }

        public IOrderedQueryable<HarvestDataModel> DefaultOrder(IQueryable<HarvestDataModel> query)
        {
            return query.OrderByDescending(x => x.DateOfHarves);
        }
    }
}
