namespace BeekeeperAssistant.Web.ViewModels.Beehives
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BeekeeperAssistant.Data.Models;

    public class CreateMultipleBeehivesInputModel : IValidatableObject
    {
        [Display(Name = "Номер на първия кошер")]
        public int StartNumber { get; set; } = 1;

        [Display(Name = "Номер на последния кошер")]
        public int EndNumber { get; set; } = 1;

        [Display(Name = "Въведете номерата на кошерите (разделени със запетая)")]
        public string NumbersSeparatedWithComma { get; set; }

        public bool UseStartEndNumber { get; set; }

        [Required(ErrorMessage = "Полето 'начин на добавяне' е задължително")]
        [Display(Name = "Изберете начин на добавяне")]
        public CreateMultipleBeehivesOptions CreateOption { get; set; }

        [Required(ErrorMessage = "Полето 'Система' е задължително!")]
        [Display(Name = "Система")]
        public BeehiveSystem BeehiveSystem { get; set; }

        [Required(ErrorMessage = "Полето 'Вид' е задължително!")]
        [Display(Name = "Вид")]
        public BeehiveType BeehiveType { get; set; }

        [Required(ErrorMessage = "Полето 'Дата на създаване' е задължително!")]
        [Display(Name = "Дата на създаване")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Полето 'Пчелин' е задължително!")]
        [Display(Name = "Пчелин")]
        public int ApiaryId { get; set; }

        public IEnumerable<KeyValuePair<int, string>> AllApiaries { get; set; }

        [Display(Name = "Прашецоуловител")]
        public bool HasPolenCatcher { get; set; }

        [Display(Name = "Решeтка за прополис")]
        public bool HasPropolisCatcher { get; set; }

        [Display(Name = "Подвижен ли е")]
        public bool IsItMovable { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errorList = new List<ValidationResult>();

            if (this.UseStartEndNumber)
            {
                if (this.StartNumber == null || this.StartNumber == 0)
                {
                    errorList.Add(new ValidationResult("Полето 'Номер на първия кошер' е задължително!"));
                }

                if (this.EndNumber == null || this.EndNumber == 0)
                {
                    errorList.Add(new ValidationResult("Полето 'Номер на последния кошер' е задължително!"));
                }

                if (this.EndNumber <= this.StartNumber)
                {
                    errorList.Add(new ValidationResult("Номерът на последния кошер трябва да бъде по-голям от номера на първия."));
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(this.NumbersSeparatedWithComma))
                {
                    errorList.Add(new ValidationResult("Полето 'Номера на кошерите' е задължително!"));
                }
            }

            return errorList;
        }
    }
}
