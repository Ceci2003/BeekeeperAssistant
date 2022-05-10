namespace BeekeeperAssistant.Web.ViewModels.Apiaries
{
    using BeekeeperAssistant.Data.Filters.Models;

    public class AllApiaryViewModel
    {
        public FilterModel UserApiariesFilter { get; set; }

        public AllApiaryUserApiariesViewModel UserApiaries { get; set; }

        public AllApiaryUserHelperApiariesViewModel UserHelperApiaries { get; set; }
    }
}
