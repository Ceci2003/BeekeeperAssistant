namespace BeekeeperAssistant.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class UsersApiaries
    {
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int ApiaryId { get; set; }

        public virtual Apiary Apiary { get; set; }
    }
}
