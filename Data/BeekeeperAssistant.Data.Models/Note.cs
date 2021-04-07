namespace BeekeeperAssistant.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using BeekeeperAssistant.Data.Common.Models;

    public class Note : BaseDeletableModel<int>
    {
        public Note()
        {
            this.Users = new HashSet<ApplicationUser>();
        }

        public string Title { get; set; }

        public string Text { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}
