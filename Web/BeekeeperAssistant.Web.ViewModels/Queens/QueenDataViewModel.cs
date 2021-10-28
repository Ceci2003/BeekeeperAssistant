namespace BeekeeperAssistant.Web.ViewModels.Queens
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AutoMapper;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class QueenDataViewModel : IMapFrom<Queen>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public int BeehiveId { get; set; }

        public int ApiaryId { get; set; }

        public Beehive Beehive { get; set; }

        public DateTime FertilizationDate { get; set; }

        public DateTime GivingDate { get; set; }

        public QueenType QueenType { get; set; }

        public string Origin { get; set; }

        public string HygenicHabits { get; set; }

        public string Temperament { get; set; }

        public QueenColor Color { get; set; }

        public QueenBreed Breed { get; set; }

        public virtual ApplicationUser User { get; set; }

        public bool HasHelpers { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Queen, QueenDataViewModel>()
                .ForMember(
                    vm => vm.HasHelpers,
                    opt => opt.MapFrom(src => src.QueenHelpers.Any()));
        }

    }
}
