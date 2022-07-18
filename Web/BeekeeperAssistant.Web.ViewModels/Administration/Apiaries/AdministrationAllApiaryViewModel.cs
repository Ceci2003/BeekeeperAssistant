namespace BeekeeperAssistant.Web.ViewModels.Administration.Apiaries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Filters.Models;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;

    // {area}{action}{controller}{FieldName?}ViewModel
    public class AdministrationAllApiaryViewModel
    {
        public FilterModel ApiariesFilter { get; set; }

        public IEnumerable<AdministrationAllApiaryAllApiariesViewModel> AllApiaries { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }
    }
}
