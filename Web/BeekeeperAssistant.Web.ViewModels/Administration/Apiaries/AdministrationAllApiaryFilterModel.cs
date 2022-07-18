namespace BeekeeperAssistant.Web.ViewModels.Administration.Apiaries
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;

    public class AdministrationAllApiaryFilterModel : IMapFrom<Apiary>, IMapFrom<ApiaryDataModel>
    {
        [Display(Name = "Номер")]
        public string Number { get; set; }

        [Display(Name = "Изтрит")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Собственик")]
        public string CreatorUserName { get; set; }

        [Display(Name = "Създаден на")]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Изтрит на")]
        public DateTime DeletedOn { get; set; }
    }
}
