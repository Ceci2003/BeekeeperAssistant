namespace BeekeeperAssistant.Web.ViewModels.Harvest
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class HarvestDatavVewModel : IMapFrom<Harvest>
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
    }
}
