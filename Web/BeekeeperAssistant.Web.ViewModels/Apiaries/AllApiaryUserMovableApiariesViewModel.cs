﻿namespace BeekeeperAssistant.Web.ViewModels.Apiaries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AllApiaryUserMovableApiariesViewModel
    {
        public IEnumerable<AllApiaryUserMovableApiariesDataViewModel> AllUserMovableApiaries { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }
    }
}
