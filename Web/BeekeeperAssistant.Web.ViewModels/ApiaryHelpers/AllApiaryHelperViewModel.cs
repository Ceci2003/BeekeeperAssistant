﻿namespace BeekeeperAssistant.Web.ViewModels.ApiaryHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AllApiaryHelperViewModel
    {
        public IEnumerable<ApiaryHelperViewModel> AllHelpers { get; set; }

        public int ApiaryId { get; set; }

        public string ApiaryNumber { get; set; }
    }
}