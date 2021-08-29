namespace BeekeeperAssistant.Web.ViewModels.Treatments
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using Microsoft.Extensions.DependencyInjection;

    public class CreateTreatmentInputModel
    {
        [Required]
        [Display(Name = "Дата на третиране")]
        public DateTime DateOfTreatment { get; set; }

        [Display(Name = "Име на третирането")]
        public string Name { get; set; }

        [Display(Name = "Бележка")]
        public string Note { get; set; }

        [Required]
        [Display(Name = "Заболяване")]
        public string Disease { get; set; }

        [Required]
        [Display(Name = "Препарат")]
        public string Medication { get; set; }

        [Required]
        [Display(Name = "Въведете като")]
        public InputAs InputAs { get; set; }

        [Required]
        [Display(Name = "Количество")]
        public double Quantity { get; set; }

        [Required]
        [Display(Name = "Дозировка")]
        public Dose Dose { get; set; }

        public IEnumerable<KeyValuePair<int, string>> Apiaries { get; set; }

        public int ApiaryId { get; set; }

        [Display(Name = "Кошери")]
        public string BeehiveNumbersSpaceSeparated { get; set; }

        [Display(Name = "Всички кошери")]
        public bool AllBeehives { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errorList = new List<ValidationResult>();
            if (!this.AllBeehives)
            {
                var beehiveRepository = validationContext.GetService<IDeletableEntityRepository<Beehive>>();
                var apiaryBeehivesNumbers = beehiveRepository.All().Where(b => b.ApiaryId == this.ApiaryId).Select(b => b.Number);
                var selectedBeehiveNumbers = this.BeehiveNumbersSpaceSeparated.Split(' ').Select(n => Convert.ToInt32(n)).ToList();

                foreach (var number in selectedBeehiveNumbers)
                {
                    if (!apiaryBeehivesNumbers.Contains(number))
                    {
                        errorList.Add(new ValidationResult($"Не съществува кошер с номер <strong>{number}</strong> в пчелина!"));
                    }
                }
            }

            return errorList;
        }
    }
}
