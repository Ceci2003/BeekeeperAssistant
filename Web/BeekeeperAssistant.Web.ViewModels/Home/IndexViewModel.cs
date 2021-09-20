namespace BeekeeperAssistant.Web.ViewModels.Home
{
    using System.Collections.Generic;

    using BeekeeperAssistant.Data.Models;

    public class IndexViewModel
    {
        public int TreatmentsCount { get; set; }

        public int InspectionsCount { get; set; }

        public int HarvestsCount { get; set; }

        public int ApiariesCount { get; set; }

        public Dictionary<ApiaryType, int> ApiariesCountByType { get; set; }

        public string ApiariesCountChartUrl { get; set; }
    }
}
