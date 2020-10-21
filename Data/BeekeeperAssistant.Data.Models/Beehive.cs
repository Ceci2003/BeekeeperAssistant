namespace BeekeeperAssistant.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using BeekeeperAssistant.Data.Common.Models;

    public class Beehive : BaseDeletableModel<int>
    {
        public virtual Apiary Apiary { get; set; }

        public int ApiaryId { get; set; }

        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public int Number { get; set; }

        public BeehiveSystem BeehiveSystem { get; set; }

        public BeehiveType BeehiveType { get; set; }

        public DateTime Date { get; set; }

        public BeehivePower BeehivePower { get; set; }

        public bool? HasDevice { get; set; }

        public bool? HasPolenCatcher { get; set; }

        public bool? HasPropolisCatcher { get; set; }
    }
}
