namespace BeekeeperAssistant.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class UserApiaries
    {
        [Key]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Key]
        public int ApiaryId { get; set; }

        public Apiary Apiary { get; set; }

    }
}
