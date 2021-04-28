namespace BeekeeperAssistant.Web.ViewModels.Beehives
{
    using System.Collections.Generic;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class AllBeehivesViewModel
    {
        public IEnumerable<BeehiveViewModel> AllBeehives { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }
    }
}
