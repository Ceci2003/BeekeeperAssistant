namespace BeekeeperAssistant.Web.ViewModels.QueenHelpers
{
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class QueenHelperViewModel : IMapFrom<QueenHelper>
    {
        public string UserUserName { get; set; }

        public string UserId { get; set; }

        public int QueenId { get; set; }

        public Access Access { get; set; }
    }
}
