namespace BeekeeperAssistant.Web.ViewModels.Treatments
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using BeekeeperAssistant.Data.Filters.Contracts;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class TreatmentDataModel : IMapFrom<Treatment>, IMapFrom<TreatmentDataModel>, IDefaultOrder<TreatmentDataModel>
    {
        public int Id { get; set; }

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

        public DateTime CreatedOn { get; set; }

        public string CreatorId { get; set; }

        public IOrderedQueryable<TreatmentDataModel> DefaultOrder(IQueryable<TreatmentDataModel> query)
        {
            return query.OrderByDescending(x => x.CreatedOn);
        }
    }
}
