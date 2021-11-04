namespace BeekeeperAssistant.Web.ViewModels.Harvest
{
    using System.Collections.Generic;

    public class AllHarvestsViewModel
    {
        public IEnumerable<HarvestDatavVewModel> AllHarvests { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }
    }
}
