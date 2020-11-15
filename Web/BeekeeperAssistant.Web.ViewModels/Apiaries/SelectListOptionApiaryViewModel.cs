namespace BeekeeperAssistant.Web.ViewModels.Apiaries
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class SelectListOptionApiaryViewModel : IMapFrom<Apiary>
    {
        public int Id { get; set; }

        public string Number { get; set; }
    }
}
