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

    public class EditQueenInutModel : IMapFrom<Queen>, IValidatableObject
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

        public int BeehiveNumber { get; set; }

        public string ApiaryNumber { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errorList = new List<ValidationResult>();

            if (this.Color != QueenColor.Unmarked)
            {
                var year = this.GivingDate.Year % 10;
                var validColor = QueenColor.White;
                var validColorBG = "Бял";

                switch (this.GivingDate.Year % 10)
                {
                    case 0:
                    case 5:
                        validColor = QueenColor.Blue;
                        validColorBG = "Син";
                        break;

                    case 1:
                    case 6:
                        validColor = QueenColor.White;
                        validColorBG = "Бял";
                        break;

                    case 2:
                    case 7:
                        validColor = QueenColor.Yellow;
                        validColorBG = "Жълт";
                        break;

                    case 3:
                    case 8:
                        validColor = QueenColor.Red;
                        validColorBG = "Червен";
                        break;

                    case 4:
                    case 9:
                        validColor = QueenColor.Green;
                        validColorBG = "Зелен";
                        break;
                }

                if (this.Color != validColor)
                {
                    errorList.Add(new ValidationResult($"Невалиден цвят - Валиден за {this.GivingDate.Year}г. е {validColorBG}!"));
                }
            }

            return errorList;
        }
    }
}
