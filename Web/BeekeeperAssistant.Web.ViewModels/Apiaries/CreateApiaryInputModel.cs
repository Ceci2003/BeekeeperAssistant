namespace BeekeeperAssistant.Web.ViewModels.Apiaries
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Web.Infrastructure.ValidationAttributes.Apiaries;
    using Microsoft.Extensions.DependencyInjection;

    public class CreateApiaryInputModel : IValidatableObject
    {
        [Required(ErrorMessage = GlobalConstants.CityCodeRequiredErrorMessage)]
        [MaxLength(GlobalConstants.MaxPartNumberLength)]
        [RegularExpression(GlobalConstants.ApiaryPartNumberRegex)]
        [Display(Name = "Номер на населеното място")]
        public string CityCode { get; set; }

        [Required(ErrorMessage = GlobalConstants.FarmNumberRequiredErrorMessage)]
        [MaxLength(GlobalConstants.MaxPartNumberLength)]
        [RegularExpression(GlobalConstants.ApiaryPartNumberRegex)]
        [Display(Name = "Номер на обект")]
        public string FarmNumber { get; set; }

        [ApiaryExistsValidation(ErrorMessage = GlobalConstants.ApiaryExistsErrorMessage)]
        public string Number => $"{this.CityCode}-{this.FarmNumber}";

        [Display(Name = "Име на пчелина")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Вид на пчелина")]
        public ApiaryType ApiaryType { get; set; }

        [Required(ErrorMessage = GlobalConstants.AddressRequiredErrorMessage)]
        [Display(Name = "Местоположение")]
        public string Adress { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errorList = new List<ValidationResult>();
            var apiaryRepository = validationContext.GetService<IDeletableEntityRepository<Apiary>>();
            var apiary = apiaryRepository.All().FirstOrDefault(a => a.Number == this.Number);

            if (apiary != null)
            {
                errorList.Add(new ValidationResult("Вече съществува пчелин с такъв номер!"));
            }

            return errorList;
        }
    }
}
