namespace BeekeeperAssistant.Web.ViewModels.Administration.Apiaries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;

    public class AllApiariesAdministrationViewModel
    {
        public IEnumerable<ApiaryViewModel> AllApiaries { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }
    }
}
