namespace BeekeeperAssistant.Web.ViewModels.BeehiveNotes
{
    using System;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class ByBeehiveIdBeehiveNoteViewModel : IMapFrom<BeehiveNote>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Color { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}
