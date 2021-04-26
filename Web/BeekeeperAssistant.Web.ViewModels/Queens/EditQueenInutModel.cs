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

    public class EditQueenInutModel : IMapFrom<Queen>
    {
        [Required]
        public int BeehiveId { get; set; }

        [Required]
        [Display(Name = "Цвят")]
        public QueenColor Color { get; set; }

        [Required]
        [Display(Name = "Порода")]
        public QueenBreed Breed { get; set; }

        [Required]
        [Display(Name = "Дата на олождане")]
        public DateTime FertilizationDate { get; set; }

        [Required]
        [Display(Name = "Дата на придаване")]
        public DateTime GivingDate { get; set; }

        [Required]
        [Display(Name = "Вид")]
        public QueenType QueenType { get; set; }

        [Display(Name = "Произход")]
        public string Origin { get; set; }

        [Display(Name = "Хигиенни навици")]
        public string HygenicHabits { get; set; }

        [Display(Name = "Нрав")]
        public string Temperament { get; set; }
    }
}
