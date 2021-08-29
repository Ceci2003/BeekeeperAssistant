namespace BeekeeperAssistant.Web.ViewModels.Beehives
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using Microsoft.Extensions.DependencyInjection;

    public class CreateBeehiveInputModel : IValidatableObject
    {
        [Required]
        [Display(Name = "Номер")]
        public int Number { get; set; }

        [Required]
        [Display(Name = "Система")]
        public BeehiveSystem BeehiveSystem { get; set; }

        [Required]
        [Display(Name = "Вид")]
        public BeehiveType BeehiveType { get; set; }

        [Required]
        [Display(Name = "Дата на създаване")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Пчелин")]
        public int ApiaryId { get; set; }

        [Required]
        [Display(Name = "Сила на кошера")]
        public BeehivePower BeehivePower { get; set; }

        public IEnumerable<KeyValuePair<int, string>> AllApiaries { get; set; }

        [Display(Name = "Апарат за майки")]
        public bool HasDevice { get; set; }

        [Display(Name = "Прашецоуловител")]
        public bool HasPolenCatcher { get; set; }

        [Display(Name = "Решeтка за прополис")]
        public bool HasPropolisCatcher { get; set; }

        [Display(Name = "Подвижен ли е")]
        public bool IsItMovable { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errorList = new List<ValidationResult>();
            var beehiveRepository = validationContext.GetService<IDeletableEntityRepository<Beehive>>();
            var beehive = beehiveRepository.All().FirstOrDefault(b => b.Number == this.Number && b.ApiaryId == this.ApiaryId);

            if (beehive != null)
            {
                errorList.Add(new ValidationResult("Вече съществува кошер с такъв номер!"));
            }

            return errorList;
        }
    }
}
