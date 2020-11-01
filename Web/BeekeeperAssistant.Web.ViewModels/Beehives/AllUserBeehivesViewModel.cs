namespace BeekeeperAssistant.Web.ViewModels.Beehives
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class AllUserBeehivesViewModel : IMapFrom<Beehive>
    {
        public IEnumerable<UserBeehiveViewModel> AllUserBeehives { get; set; }

        public int ApiaryId { get; set; }
    }
}
