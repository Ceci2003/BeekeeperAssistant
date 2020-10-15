namespace BeekeeperAssistant.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class UsersApiaries
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int ApiaryId { get; set; }

        public Apiary Apiary { get; set; }
    }
}
