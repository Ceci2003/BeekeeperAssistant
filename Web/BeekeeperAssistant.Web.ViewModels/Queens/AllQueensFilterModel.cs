using BeekeeperAssistant.Data.Models;
using BeekeeperAssistant.Services.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeekeeperAssistant.Web.ViewModels.Queens
{
    public class AllQueensFilterModel : IMapFrom<Queen>, IMapFrom<QueenDataModel>
    {
        [Display(Name = "Порода")]
        public QueenBreed Breed { get; set; }

        [Display(Name = "Произход")]
        public string Origin { get; set; }

        [Display(Name = "Номер на кошер")]
        public int BeehiveNumber { get; set; }

        [Display(Name = "Номер на пчелин")]
        public string BeehiveApiaryNumber { get; set; }

        [Display(Name = "Дата на придаване")]
        public DateTime FertilizationDate { get; set; }

        [Display(Name = "Цвят")]
        public QueenColor Color { get; set; }

    }
}
