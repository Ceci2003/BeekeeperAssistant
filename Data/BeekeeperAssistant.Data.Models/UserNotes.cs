namespace BeekeeperAssistant.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class UserNotes
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int NoteId { get; set; }

        public UserNote Note { get; set; }
    }
}
