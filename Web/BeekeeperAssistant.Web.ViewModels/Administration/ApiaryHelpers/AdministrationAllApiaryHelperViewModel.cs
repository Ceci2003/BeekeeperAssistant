﻿namespace BeekeeperAssistant.Web.ViewModels.Administration.ApiaryHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AdministrationAllApiaryHelperViewModel
    {
        public IEnumerable<ApiaryHelperAdministrationViewModel> AllApiariesHelpers { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }
    }
}
