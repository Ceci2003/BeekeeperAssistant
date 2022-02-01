namespace BeekeeperAssistant.Web.ViewModels.Beehives
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AutoMapper;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class AllByMovableApiaryIdBeehiveViewModel : IMapFrom<TemporaryApiaryBeehive>
    {
        public int ApiaryId { get; set; }

        public string ApiaryNumber { get; set; }

        public string ApiaryName { get; set; }

        public Access ApiaryAccess { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        [IgnoreMap]
        public IEnumerable<ByMovableApiaryIdBeehiveViewModel> AllBeehives { get; set; }
    }
}
