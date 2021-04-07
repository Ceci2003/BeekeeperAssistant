namespace BeekeeperAssistant.Web.ViewModels.Apiaries
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class ApiaryDataViewModel : IMapFrom<Apiary>
    {
        public string Id { get; set; }

        public string Number { get; set; }

        public ApiaryType ApiaryType { get; set; }

        public string Adress { get; set; }

        public string Name { get; set; }

        // Change with Beehive view model
        public IEnumerable<Beehive> Beehives { get; set; }
    }
}
