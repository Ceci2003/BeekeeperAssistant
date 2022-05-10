namespace BeekeeperAssistant.Web.ViewModels.Inspections
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class AllByBeehiveIdInspectionFilterModel : IMapFrom<Inspection>, IMapFrom<InspectionDataModel>
    {
        [Display(Name = "Дата на прегледа")]
        public DateTime DateOfInspection { get; set; }

        [Display(Name = "Вид на прегледа")]
        public InspectionType InspectionType { get; set; }

        [Display(Name = "Роил ли се е")]
        public bool Swarmed { get; set; }

        [Display(Name = "Сила на кошера")]
        public BeehivePower BeehivePower { get; set; }
    }
}
