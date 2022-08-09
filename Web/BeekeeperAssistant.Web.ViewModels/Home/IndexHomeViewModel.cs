namespace BeekeeperAssistant.Web.ViewModels.Home
{
    using System;
    using System.Collections.Generic;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Web.ViewModels.UserTasks;

    public class IndexHomeViewModel
    {
        public int TreatmentsCount { get; set; }

        public int InspectionsCount { get; set; }

        public int HarvestsCount { get; set; }

        public int ApiariesCount { get; set; }

        public Dictionary<ApiaryType, int> ApiariesCountByType { get; set; }

        public string ApiariesCountChartUrl { get; set; }

        public int BeehivesCount { get; set; }

        public Dictionary<BeehivePower, int> BeehivesCountByPower { get; set; }

        public string BeehivesCountChartUrl { get; set; }

        public int QueensCount { get; set; }

        public Dictionary<int, int> QueensCountByGivingDate { get; set; }

        public string QueensCountByGivingDateChartUrl { get; set; }

        public List<string> QueenChartColors { get; set; }

        public IEnumerable<UserTaskViewModel> LastUserTasks { get; set; }

        public Dictionary<string, int> UserTasksCountByColor { get; set; }
    }
}
