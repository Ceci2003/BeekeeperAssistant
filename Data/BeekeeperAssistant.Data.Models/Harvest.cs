namespace BeekeeperAssistant.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using BeekeeperAssistant.Data.Common.Models;

    public class Harvest : BaseDeletableModel<int>
    {
        public int BeehiveId { get; set; }

        public virtual Beehive Beehive { get; set; }

        public string HarvestName { get; set; }

        public DateTime DateOfHarves { get; set; }

        public string Product { get; set; }

        public HoneyType HoneyType { get; set; }

        public string Note { get; set; }

        public int Amount { get; set; }
    }
}
