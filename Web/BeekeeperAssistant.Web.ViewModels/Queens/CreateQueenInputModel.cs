﻿namespace BeekeeperAssistant.Web.ViewModels.Queens
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class CreateQueenInputModel
    {
        [Required]
        public int BeehiveId { get; set; }

        [Required(ErrorMessage = "Полето 'Цвят' е задължително!")]
        [Display(Name = "Цвят")]
        public QueenColor Color { get; set; }

        [Required(ErrorMessage = "Полето 'Порода' е задължително!")]
        [Display(Name = "Порода")]
        public QueenBreed Breed { get; set; }

        [Required(ErrorMessage = "Полето 'Дата на олождане' е задължително!")]
        [Display(Name = "Дата на олождане")]
        public DateTime FertilizationDate { get; set; }

        [Required(ErrorMessage = "Полето 'Дата на придаване' е задължително!")]
        [Display(Name = "Дата на придаване")]
        public DateTime GivingDate { get; set; }

        [Required(ErrorMessage = "Полето 'Вид' е задължително!")]
        [Display(Name = "Вид")]
        public QueenType QueenType { get; set; }

        [Display(Name = "Произход")]
        public string Origin { get; set; }

        [Display(Name = "Хигиенни навици")]
        public string HygenicHabits { get; set; }

        [Display(Name = "Нрав")]
        public string Temperament { get; set; }

        public int BeehiveNumber { get; set; }

        public int ApiaryId{ get; set; }

        public string ApiaryNumber { get; set; }

        public string ApiaryName { get; set; }
    }
}
