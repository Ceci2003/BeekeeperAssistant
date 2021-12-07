namespace BeekeeperAssistant.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using BeekeeperAssistant.Data.Common.Models;

    public class Harvest : BaseDeletableModel<int>
    {
        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public string HarvestName { get; set; }

        public DateTime DateOfHarves { get; set; }

        public string Note { get; set; }

        public HarvestProductType HarvestProductType { get; set; }

        public HoneyType HoneyType { get; set; }

        public double Quantity { get; set; }

        public Unit Unit { get; set; }

        public virtual ICollection<HarvestedBeehive> HarvestedBeehives { get; set; }
    }
}
