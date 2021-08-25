namespace BeekeeperAssistant.Web.ViewModels.Treatments
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class TreatmentDataViewModel : IMapFrom<Treatment>
    {
        public int TreatmentId { get; set; }

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
    }
}
