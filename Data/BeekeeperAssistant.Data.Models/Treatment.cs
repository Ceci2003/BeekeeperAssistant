namespace BeekeeperAssistant.Data.Models
{
    using System;
    using System.Collections.Generic;

    using BeekeeperAssistant.Data.Common.Models;

    public class Treatment : BaseDeletableModel<int>
    {
        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public DateTime DateOfTreatment { get; set; }

        public string Name { get; set; }

        public string Note { get; set; }

        public string Disease { get; set; }

        public string Medication { get; set; }

        public InputAs InputAs { get; set; }

        public double Quantity { get; set; }

        public Dose Doses { get; set; }

        public ICollection<TreatedBeehive> TreatedBeehives { get; set; }
    }
}
