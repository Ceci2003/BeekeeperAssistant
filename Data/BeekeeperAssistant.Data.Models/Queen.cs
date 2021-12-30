namespace BeekeeperAssistant.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    using BeekeeperAssistant.Data.Common.Models;

    public class Queen : BaseDeletableModel<int>
    {
        [ForeignKey(nameof(Beehive))]
        public int BeehiveId { get; set; }

        public virtual Beehive Beehive { get; set; }

        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public string OwnerId { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        public DateTime FertilizationDate { get; set; }

        public DateTime GivingDate { get; set; }

        public QueenType QueenType { get; set; }

        public string Origin { get; set; }

        public string HygenicHabits { get; set; }

        public string Temperament { get; set; }

        public bool IsBookMarked { get; set; }

        public QueenColor Color { get; set; }

        public QueenBreed Breed { get; set; }

        public virtual ICollection<QueenHelper> QueenHelpers { get; set; }
    }
}
