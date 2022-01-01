namespace BeekeeperAssistant.Web.ViewModels.ApiaryNotes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class ByApiaryIdApiaryNoteViewModel : IMapFrom<ApiaryNote>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Color { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}
