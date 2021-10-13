namespace BeekeeperAssistant.Web.ViewModels.Apiaries
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class AllApiariesViewModel
    {
        public AllUserApiariesViewModel UserApiaries { get; set; }

        public AllHelperApiariesViewModel UserHelperApiaries { get; set; }
    }
}
