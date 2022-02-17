namespace BeekeeperAssistant.Web.ViewModels.ApiaryNotes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using AutoMapper;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class AllByApiaryIdApiaryNoteViewModel : IMapFrom<Apiary>
    {
        public int ApiaryId { get; set; }

        public string ApiaryNumber { get; set; }

        public string ApiaryName { get; set; }

        public ApiaryType ApiaryType { get; set; }

        public Access ApiaryAccess { get; set; }

        [IgnoreMap]
        public IEnumerable<ByApiaryIdApiaryNoteViewModel> AllNotes { get; set; }
    }
}
