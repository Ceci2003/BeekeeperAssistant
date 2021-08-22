namespace BeekeeperAssistant.Web.ViewModels.Treatments
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    using BeekeeperAssistant.Data.Models;

    public class CreateTreatmentInputModel
    {
        [Required]
        [Display(Name = "Дата на третиране")]
        public DateTime DateOfTreatment { get; set; }

        [Display(Name = "Име на третиарането")]
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
        [Display(Name = "Виведете като")]
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

        public int BeehiveId { get; set; }

        [Display(Name = "Всички кошери")]
        public bool AllBeehives { get; set; }
    }
}
