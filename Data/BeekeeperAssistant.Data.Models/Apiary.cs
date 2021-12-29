namespace BeekeeperAssistant.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using BeekeeperAssistant.Data.Common.Models;

    public class Apiary : BaseDeletableModel<int>
    {
        public Apiary()
        {
            this.Beehives = new HashSet<Beehive>();
            this.ApiaryHelpers = new HashSet<ApiaryHelper>();
        }

        public string Number { get; set; }

        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public string Name { get; set; }

        public ApiaryType ApiaryType { get; set; }

        public string Adress { get; set; }

        public bool IsBookMarked { get; set; }

        public bool IsRegistered { get; set; }

        public virtual ICollection<Beehive> Beehives { get; set; }

        public virtual ICollection<ApiaryHelper> ApiaryHelpers { get; set; }
    }
}
