namespace BeekeeperAssistant.Web.ViewModels.Queens
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class CreateQueenInputModel : IMapFrom<Queen>
    {
        [Required]
        public DateTime FertilizationDate { get; set; }

        [Required]
        public DateTime GivingDate { get; set; }

        [Required]
        public QueenType QueenType { get; set; }

        public string Origin { get; set; }

        public string HygenicHabits { get; set; }

        public string Temperament { get; set; }

        [Required]
        public QueenColor Color { get; set; }

        [Required]
        public QueenBreed Breed { get; set; }

        [Required]
        public int BeehiveId { get; set; }
    }
}
