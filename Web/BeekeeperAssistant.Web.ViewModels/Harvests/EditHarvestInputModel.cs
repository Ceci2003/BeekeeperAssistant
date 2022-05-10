namespace BeekeeperAssistant.Web.ViewModels.Harvests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class EditHarvestInputModel : IMapFrom<Harvest>
    {
        [Display(Name = "Име")]
        public string HarvestName { get; set; }

        [Required(ErrorMessage = "Полето 'Дата на добива' е задължително!")]
        [Display(Name = "Дата на добива")]
        public DateTime DateOfHarves { get; set; }

        [Display(Name = "Бележка")]
        public string Note { get; set; }

        [Required(ErrorMessage = "Полето 'Добит продукт' е задължително!")]
        [Display(Name = "Добит продукт")]
        public HarvestProductType HarvestProductType { get; set; }

        [Display(Name = "Вид мед")]
        public HoneyType HoneyType { get; set; }

        [Required(ErrorMessage = "Полето 'Количество' е задължително!")]
        [Display(Name = "Количество")]
        public string QuantityText { get; set; }

        public double Quantity { get; set; }

        [Display(Name = "Еденица")]
        public Unit Unit { get; set; }

        public IEnumerable<KeyValuePair<int, string>> Apiaries { get; set; }

        [Display(Name = "Пчелин")]
        public int ApiaryId { get; set; }

        [Display(Name = "Кошери")]
        public string BeehiveNumbersSpaceSeparated { get; set; }

        [Display(Name = "Всички кошери")]
        public bool AllBeehives { get; set; }

        public int? BeehiveId { get; set; }

        public int BeehiveNumber { get; set; }

        public string ApiaryNumber { get; set; }
    }
}
