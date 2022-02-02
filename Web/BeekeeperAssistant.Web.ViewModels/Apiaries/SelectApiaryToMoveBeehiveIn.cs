namespace BeekeeperAssistant.Web.ViewModels.Apiaries
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using Microsoft.Extensions.DependencyInjection;

    public class SelectApiaryToMoveBeehiveIn : IValidatableObject
    {
        public int BeehiveId { get; set; }

        public int BeehiveNumber { get; set; }

        public int BeehiveApiaryId { get; set; }

        public string BeehiveApiaryNumber { get; set; }

        public string BeehiveApiaryName { get; set; }

        [Display(Name = "Пчелин")]
        public int SelectedApiaryId { get; set; }

        public IEnumerable<KeyValuePair<int, string>> AllApiaries { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errorList = new List<ValidationResult>();
            var beehiveRepository = validationContext.GetService<IDeletableEntityRepository<Beehive>>();
            var beehive = beehiveRepository.All().FirstOrDefault(b => b.Id == this.BeehiveId && b.ApiaryId == this.SelectedApiaryId);

            if (beehive != null)
            {
                errorList.Add(new ValidationResult("Кошерът вече се намира в избрания пчелин!"));
            }

            return errorList;
        }
    }
}
