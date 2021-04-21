namespace BeekeeperAssistant.Web.ViewModels.Queens
{
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using System;

    public class QueenViewModel : IMapFrom<Queen>
    {
        public int Id { get; set; }

        public int BeehiveId { get; set; }

        public int BeehiveNumber { get; set; }

        public string BeehiveApiaryNumber { get; set; }

        public QueenColor Color { get; set; }

        public DateTime GivingDate { get; set; }

        public string Origin { get; set; }

        public QueenBreed Breed { get; set; }
    }
}
