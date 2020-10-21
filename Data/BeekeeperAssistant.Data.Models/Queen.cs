namespace BeekeeperAssistant.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using BeekeeperAssistant.Data.Common.Models;

    public class Queen : BaseDeletableModel<int>
    {
        public int BeehiveId { get; set; }

        public virtual Beehive Beehive { get; set; }

        public DateTime FertilizationDate { get; set; }

        public DateTime GivingDate { get; set; }

        public QueenType QueenType { get; set; }

        public string Origin { get; set; }

        public string HygenicHabits { get; set; }

        public string Temperament { get; set; }

        public bool Marked { get; set; }

        public string Color { get; set; }
    }
}
