namespace BeekeeperAssistant.Web.ViewModels.Beehives
{
    using System.Collections.Generic;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class AllBeehivesViewModel : IMapFrom<Beehive>
    {
        public IEnumerable<BeehiveViewModel> AllUserBeehives { get; set; }

        public int ApiaryId { get; set; }
    }
}
