namespace BeekeeperAssistant.Web.ViewModels.Apiaries
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using Microsoft.Extensions.DependencyInjection;

    public class EditApiaryInputModel : IMapFrom<Apiary>, IMapFrom<ApiaryDataModel>
    {
        [RegularExpression(GlobalConstants.ApiaryPartNumberRegex, ErrorMessage = "Пощенският код не съществува или не е валиден. Трябва да въведете 4 цифри.")]
        [Display(Name = "Пощенски код")]
        public string CityCode { get; set; }

        [RegularExpression(GlobalConstants.ApiaryPartNumberRegex, ErrorMessage = "Невалиден номер на пчелин. Трябва да въведете 4 цифри.")]
        [Display(Name = "Номер на пчелин")]
        public string FarmNumber { get; set; }

        public string Number { get; set; }

        [Display(Name = "Име на пчелина")]
        public string Name { get; set; }

        [Display(Name = "Вид на пчелина")]
        public ApiaryType ApiaryType { get; set; }

        [Required(ErrorMessage = GlobalConstants.AddressRequiredErrorMessage)]
        [Display(Name = "Местоположение")]
        public string Adress { get; set; }

        [Display(Name = "Регистриран ли е пчелина?")]
        public bool IsRegistered { get; set; }

        [Display(Name = "Статус на пчелина")]
        public bool IsClosed { get; set; }

        [Display(Name = "Дата на отваряне")]
        public DateTime? OpeningDate { get; set; }

        [Display(Name = "Дата на затваряне")]
        public DateTime? ClosingDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errorList = new List<ValidationResult>();
            var apiaryRepository = validationContext.GetService<IDeletableEntityRepository<Apiary>>();
            var apiaryByNumber = apiaryRepository.All().FirstOrDefault(a => a.Number == this.Number);
            var apiaryByName = apiaryRepository.All().FirstOrDefault(a => a.Name == this.Name); // !TODO make filter by user - Names may be repeated for different users.

            if (apiaryByNumber != null)
            {
                errorList.Add(new ValidationResult("Вече съществува пчелин с такъв номер в системата!"));
            }

            if (this.IsRegistered)
            {
                if (string.IsNullOrEmpty(this.CityCode))
                {
                    errorList.Add(new ValidationResult("Въведете пощенски код. Трябва да въведете 4 цифри."));
                }

                if (string.IsNullOrEmpty(this.FarmNumber))
                {
                    errorList.Add(new ValidationResult("Въведете номер на пчелина. Трябва да въведете 4 цифри."));
                }
            }
            else
            {
                if (apiaryByName != null)
                {
                    errorList.Add(new ValidationResult("Вече съществува пчелин със зададеното име в системата!"));
                }

                if (string.IsNullOrEmpty(this.Name))
                {
                    errorList.Add(new ValidationResult("За нерегистриран пчелин трябва да въведете име."));
                }
            }

            if (this.ApiaryType == ApiaryType.Movable)
            {
                if (this.OpeningDate >= this.ClosingDate)
                {
                    errorList.Add(new ValidationResult("Датата на затваряне трябва да бъде по-късна от датата на отваряне!"));
                }
                else if (this.OpeningDate == null)
                {
                    errorList.Add(new ValidationResult("Въведете дата на отваряне!"));
                }
                else if (this.ClosingDate == null)
                {
                    errorList.Add(new ValidationResult("Въведете дата на затваряне!"));
                }
            }

            return errorList;
        }
    }
}
