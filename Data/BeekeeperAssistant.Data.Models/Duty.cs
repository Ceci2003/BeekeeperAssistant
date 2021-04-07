namespace BeekeeperAssistant.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using BeekeeperAssistant.Data.Common.Models;

    public class Duty : BaseDeletableModel<int>
    {
        public Duty()
        {
            this.Users = new HashSet<ApplicationUser>();
        }

        public string Title { get; set; }

        public string Text { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsCompleted { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}
