namespace BeekeeperAssistant.Services.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using AutoMapper;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class BeehiveDataExcelExportModel : IMapFrom<Beehive>, IHaveCustomMappings
    {
        public string ApiaryNumber { get; set; }

        public int Number { get; set; }

        public BeehiveSystem BeehiveSystem { get; set; }

        public BeehiveType BeehiveType { get; set; }

        public DateTime Date { get; set; }

        public BeehivePower BeehivePower { get; set; }

        public bool HasPolenCatcher { get; set; }

        public bool HasPropolisCatcher { get; set; }

        public bool IsItMovable { get; set; }

        public QueenColor? QueenColor { get; set; }

        public DateTime? QueenGivingDate { get; set; }

        public QueenType? QueenQueenType { get; set; }

        public string QueenOrigin { get; set; }

        public QueenBreed? QueenBreed { get; set; }

        public string QueenTemperament { get; set; }

        public string QueenHygenicHabits { get; set; }

        public bool HasQueen { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Beehive, BeehiveDataExcelExportModel>()
                .ForMember(
                vm => vm.HasQueen,
                opt => opt.MapFrom(src => src.Queen != null));
        }
    }
}
