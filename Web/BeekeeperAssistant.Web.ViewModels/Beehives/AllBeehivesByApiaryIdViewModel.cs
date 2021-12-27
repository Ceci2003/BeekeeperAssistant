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

    public class AllBeehivesByApiaryIdViewModel : IMapFrom<Apiary>
    {
        public int Id { get; set; }

        public string CreatorId { get; set; }

        public string Number { get; set; }

        public Access ApiaryAccess { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        [IgnoreMap]
        public IEnumerable<BeehiveByApiaryIdViewModel> AllBeehives { get; set; }
    }
}
