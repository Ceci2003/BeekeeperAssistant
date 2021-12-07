namespace BeekeeperAssistant.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    using BeekeeperAssistant.Data.Common.Models;

    public class Beehive : BaseDeletableModel<int>
    {
        public int ApiaryId { get; set; }

        public virtual Apiary Apiary { get; set; }

        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public int Number { get; set; }

        public BeehiveSystem BeehiveSystem { get; set; }

        public BeehiveType BeehiveType { get; set; }

        public DateTime Date { get; set; }

        public BeehivePower BeehivePower { get; set; }

        public bool HasDevice { get; set; }

        public bool HasPolenCatcher { get; set; }

        public bool HasPropolisCatcher { get; set; }

        public bool IsBookMarked { get; set; }

        public bool IsItMovable { get; set; }

        [ForeignKey(nameof(Queen))]
        public int? QueenId { get; set; }

        public virtual Queen Queen { get; set; }

        public virtual ICollection<Inspection> Inspections { get; set; }

        public virtual ICollection<HarvestedBeehive> HarvestedBeehives { get; set; }

        public virtual ICollection<TreatedBeehive> TreatedBeehives { get; set; }

        public virtual ICollection<BeehiveHelper> BeehiveHelpers { get; set; }
    }
}
