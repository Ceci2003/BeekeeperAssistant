namespace BeekeeperAssistant.Web.ViewModels.Harvest
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using BeekeeperAssistant.Data.Models;

    public class CreateHarvestInputModel
    {
        [Required]
        public int BeehiveId { get; set; }

        [Display(Name = "Име")]
        public string HarvestName { get; set; }

        [Required]
        [Display(Name = "Дата на добива")]
        public DateTime DateOfHarves { get; set; }

        [Required]
        [Display(Name = "Добит продукт")]
        public string Product { get; set; }

        [Display(Name = "Вид мед")]
        public HoneyType HoneyType { get; set; }

        [Display(Name = "Бележка")]
        public string Note { get; set; }

        [Required]
        [Display(Name = "Количество")]
        public int Amount { get; set; }
    }
}
