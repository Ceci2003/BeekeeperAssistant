﻿namespace BeekeeperAssistant.Web.ViewModels.Treatments
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using Microsoft.Extensions.DependencyInjection;

    public class CreateTreatmentInputModel : IValidatableObject
    {
        [Required(ErrorMessage = "Полето 'Дата на третиране' е задължително!")]
        [Display(Name = "Дата на третиране")]
        public DateTime DateOfTreatment { get; set; }

        [Display(Name = "Име на третирането")]
        public string Name { get; set; }

        [Display(Name = "Бележка")]
        public string Note { get; set; }

        [Required(ErrorMessage = "Полето 'Превенция на' е задължително!")]
        [Display(Name = "Превенция на")]
        public string Disease { get; set; }

        [Required(ErrorMessage = "Полето 'Препарат' е задължително!")]
        [Display(Name = "Препарат")]
        public string Medication { get; set; }

        [Required(ErrorMessage = "Полето 'Въведете като' е задължително!")]
        [Display(Name = "Въведете като")]
        public InputAs InputAs { get; set; }

        [Required(ErrorMessage = "Полето 'Количество' е задължително!")]
        [Display(Name = "Количество")]
        public double Quantity { get; set; }

        [Required(ErrorMessage = "Полето 'Дозировка' е задължително!")]
        [Display(Name = "Дозировка")]
        public Dose Dose { get; set; }

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
                        errorList.Add(new ValidationResult($"Номерата трябва да бъдата разделени с интервал! Пример: '12 5 34'"));
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
