﻿namespace BeekeeperAssistant.Web.ViewModels.Beehives
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using AutoMapper;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using BeekeeperAssistant.Web.ViewModels.Harvest;
    using BeekeeperAssistant.Web.ViewModels.Inspections;
    using BeekeeperAssistant.Web.ViewModels.Treatments;

    public class ByIdBeehiveViewModel : IMapFrom<Beehive>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string ApiaryNumber { get; set; }

        public int Number { get; set; }

        public BeehiveSystem BeehiveSystem { get; set; }

        public BeehiveType BeehiveType { get; set; }

        public DateTime Date { get; set; }

        public BeehivePower BeehivePower { get; set; }

        public bool HasDevice { get; set; }

        public bool HasPolenCatcher { get; set; }

        public bool HasPropolisCatcher { get; set; }

        public bool IsItMovable { get; set; }

        public string CreatorId { get; set; }

        public int? QueenId { get; set; }

        public Access BeehiveAccess { get; set; }

        public bool HasHelpers { get; set; }

        public bool HasQueen { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Beehive, ByIdBeehiveViewModel>()
                .ForMember(
                vm => vm.HasQueen,
                opt => opt.MapFrom(src => src.Queen != null))
                .ForMember(
                vm => vm.HasHelpers,
                opt => opt.MapFrom(src => src.BeehiveHelpers.Any()));
        }
    }
}