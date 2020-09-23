namespace BeekeeperAssistant.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class UserTasks
    {
        [Key]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Key]
        public int TaskId { get; set; }

        public UserTask Task { get; set; }

    }
}
