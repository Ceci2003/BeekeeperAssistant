namespace BeekeeperAssistant.Web.ViewModels.Beehives
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using Microsoft.Extensions.DependencyInjection;

    public class ChooseNewNumberForBeehiveInputModel : IValidatableObject
    {
        public int BeehiveId { get; set; }

        [Display(Name = "Нов номер")]
        public int BeehiveNumber { get; set; }

        public int BeehiveApiaryId { get; set; }

        public string BeehiveApiaryNumber { get; set; }

        public string BeehiveApiaryName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errorList = new List<ValidationResult>();
            var beehiveRepository = validationContext.GetService<IDeletableEntityRepository<Beehive>>();
            var beehive = beehiveRepository.All().FirstOrDefault(b => b.Number == this.BeehiveNumber && b.ApiaryId == this.BeehiveApiaryId);

            if (beehive != null)
            {
                errorList.Add(new ValidationResult("Въведеният номер съществува в пчелина!"));
            }

            return errorList;
        }
    }
}
