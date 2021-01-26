namespace BeekeeperAssistant.Web.ViewModels.Beehives
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class BeehiveDataViewModel : IMapFrom<Beehive>
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public BeehiveSystem BeehiveSystem { get; set; }

        public BeehiveType BeehiveType { get; set; }

        public DateTime CreationDate { get; set; }

        public BeehivePower BeehivePower { get; set; }

        public int ApiId { get; set; }

        public Apiary Apiary { get; set; }

        public string ApiaryNumber { get; set; }

        public bool HasDevice { get; set; }

        public bool HasPolenCatcher { get; set; }

        public bool HasPropolisCatcher { get; set; }
    }
}
