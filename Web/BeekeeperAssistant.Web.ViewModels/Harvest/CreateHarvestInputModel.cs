namespace BeekeeperAssistant.Web.ViewModels.Harvest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using Microsoft.Extensions.DependencyInjection;

    public class CreateHarvestInputModel : IValidatableObject
    {
        [Display(Name = "Име")]
        public string HarvestName { get; set; }

        [Required]
        [Display(Name = "Дата на добива")]
        public DateTime DateOfHarves { get; set; }

        [Display(Name = "Бележка")]
        public string Note { get; set; }

        [Required]
        [Display(Name = "Добит продукт")]
        public HarvestProductType HarvestProductType { get; set; }

        [Display(Name = "Вид мед")]
        public HoneyType HoneyType { get; set; }

        [Display(Name = "Количество")]
        public string QuantityText { get; set; }

        public double Quantity { get; set; }

        [Display(Name = "Еденица")]
        public Unit Unit { get; set; }

        public IEnumerable<KeyValuePair<int, string>> Apiaries { get; set; }

        public int ApiaryId { get; set; }

        [Display(Name = "Кошери")]
        public string BeehiveNumbersSpaceSeparated { get; set; }

        [Display(Name = "Всички кошери")]
        public bool AllBeehives { get; set; }

        public int? BeehiveId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errorList = new List<ValidationResult>();
            if (this.BeehiveId == null && !this.AllBeehives)
            {
                if (this.BeehiveNumbersSpaceSeparated != null)
                {
                    try
                    {
                        var beehiveRepository = validationContext.GetService<IDeletableEntityRepository<Beehive>>();
                        var apiaryBeehivesNumbers = beehiveRepository.All().Where(b => b.ApiaryId == this.ApiaryId).Select(b => b.Number);
                        var selectedBeehiveNumbers = this.BeehiveNumbersSpaceSeparated.Split(' ').Select(n => Convert.ToInt32(n)).ToList();

                        foreach (var number in selectedBeehiveNumbers)
                        {
                            if (!apiaryBeehivesNumbers.Contains(number))
                            {
                                errorList.Add(new ValidationResult($"Не съществува кошер с номер {number} в пчелина!"));
                            }
                        }
                    }
                    catch (Exception)
                    {
                        errorList.Add(new ValidationResult($"Номерата на кошерите не са въведени правилно!"));
                    }
                }
                else
                {
                    errorList.Add(new ValidationResult($"Изберете кошери!"));
                }
            }

            return errorList;
        }
    }
}
