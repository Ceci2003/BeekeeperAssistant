using System;
using System.Collections.Generic;
using System.Text;

namespace BeekeeperAssistant.Data.Models
{
    public class UserNotes
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int NoteId { get; set; }

        public UserNote Note { get; set; }
    }
}
