namespace BeekeeperAssistant.Web.ViewModels.Apiaries
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;

    using AutoMapper;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services;
    using BeekeeperAssistant.Services.Mapping;
    using BeekeeperAssistant.Web.ViewModels.Beehives;

    // {area}{action}{controller}{FieldName?}ViewModel
    public class ByNumberApiaryViewModel : IMapFrom<Apiary>, IHaveCustomMappings
    {
        public int Id { get; set; }

        [Display(Name = "Номер")]
        public string Number { get; set; }

        [Display(Name = "Вид")]
        public ApiaryType ApiaryType { get; set; }

        public ApplicationUser Creator { get; set; }

        public string CreatorId { get; set; }

        [Display(Name = "Адрес")]
        public string Adress { get; set; }

        [Display(Name = "Име")]
        public string Name { get; set; }

        public ForecastResult ForecastResult { get; set; }

        public bool HasHelpers { get; set; }

        public Access ApiaryAccess { get; set; }

        public bool IsRegistered { get; set; }

        public bool IsClosed { get; set; }

        public DateTime OpeningDate { get; set; }

        public DateTime ClosingDate { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Apiary, ByNumberApiaryViewModel>()
                .ForMember(
                    vm => vm.HasHelpers,
                    opt => opt.MapFrom(src => src.ApiaryHelpers.Any()));
        }
    }
}
