namespace BeekeeperAssistant.Web.ViewModels.Queens
{
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class UserQueenViewModel : IMapFrom<Queen>
    {
        public string Origin { get; set; }

        public string HygenicHabits { get; set; }

        public string Temperament { get; set; }

        public QueenColor Color { get; set; }

        public QueenBreed Breed { get; set; }
    }
}
