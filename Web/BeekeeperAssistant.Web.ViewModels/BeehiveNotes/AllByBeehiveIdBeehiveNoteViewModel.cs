namespace BeekeeperAssistant.Web.ViewModels.BeehiveNotes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using AutoMapper;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class AllByBeehiveIdBeehiveNoteViewModel : IMapFrom<Beehive>
    {
        public int BeehiveId { get; set; }

        public int Number { get; set; }

        public int ApiaryId { get; set; }

        public string ApiaryNumber { get; set; }

        public string ApiaryName { get; set; }

        public Access BeehiveAccess { get; set; }

        [IgnoreMap]
        public IEnumerable<ByBeehiveIdBeehiveNoteViewModel> AllNotes { get; set; }
    }
}
