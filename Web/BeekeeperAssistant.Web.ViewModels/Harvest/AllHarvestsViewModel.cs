namespace BeekeeperAssistant.Web.ViewModels.Harvest
{
    using BeekeeperAssistant.Data.Models;
    using System.Collections.Generic;

    public class AllHarvestsViewModel
    {
        public IEnumerable<HarvestDatavVewModel> AllHarvests { get; set; }

        public int BeehiveId { get; set; }

        public int BeehiveNumber { get; set; }

        public string ApiaryNumber { get; set; }

        public Access BeehiveAccess { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }
    }
}
