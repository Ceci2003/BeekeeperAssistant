using BeekeeperAssistant.Data.Models;
using BeekeeperAssistant.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeekeeperAssistant.Web.ViewModels.Queens
{
    public class QueenDataViewModel : IMapFrom<Queen>
    {
        public DateTime FertilizationDate { get; set; }

        public DateTime GivingDate { get; set; }

        public QueenType QueenType { get; set; }

        public string Origin { get; set; }

        public string HygenicHabits { get; set; }

        public string Temperament { get; set; }

        public QueenColor Color { get; set; }

        public QueenBreed Breed { get; set; }
    }
}
