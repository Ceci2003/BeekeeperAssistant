namespace BeekeeperAssistant.Web.ViewModels.Beehives
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class BeehiveViewModel : IMapFrom<Beehive>
    {
        public int Id { get; set; }

        public string ApiaryNumber { get; set; }

        public int Number { get; set; }

        public BeehivePower BeehivePower { get; set; }

        public BeehiveSystem BeehiveSystem { get; set; }

        public BeehiveType BeehiveType { get; set; }

    }
}
