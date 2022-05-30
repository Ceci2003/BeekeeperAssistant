namespace BeekeeperAssistant.Web.ViewModels.Treatments
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class AllByBeehiveIdTreatmentFilterModel : IMapFrom<Treatment>, IMapFrom<TreatmentDataModel>
    {
        [Display(Name = "Дата на третиране")]
        public DateTime DateOfTreatment { get; set; }

        [Display(Name = "Заболяване")]
        public string Disease { get; set; }

        [Display(Name = "Дозировка")]
        public Dose Dose { get; set; }

        [Display(Name = "Количество")]
        public double Quantity { get; set; }

        [Display(Name = "Въведете като")]
        public InputAs InputAs { get; set; }

        [Display(Name = "Име на третирането")]
        public string Name { get; set; }

        [Display(Name = "Препарат")]
        public string Medication { get; set; }

    }
}
