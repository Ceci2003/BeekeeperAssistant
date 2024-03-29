﻿namespace BeekeeperAssistant.Data.Models
{
    using System;
    using System.Collections.Generic;

    using BeekeeperAssistant.Data.Common.Models;

    public class Treatment : BaseDeletableModel<int>
    {
        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public string OwnerId { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        public DateTime DateOfTreatment { get; set; }

        public string Name { get; set; }

        public string Note { get; set; }

        public string Disease { get; set; }

        public string Medication { get; set; }

        public InputAs InputAs { get; set; }

        public double Quantity { get; set; }

        public Dose Dose { get; set; }

        public virtual ICollection<TreatedBeehive> TreatedBeehives { get; set; }
    }
}
