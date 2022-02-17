namespace BeekeeperAssistant.Web.ViewModels.BeehiveMarkFlags
{
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class BeehivemarkFlagViewModel : IMapFrom<BeehiveMarkFlag>
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public MarkFlagType FlagType { get; set; }
    }
}
