﻿namespace BeekeeperAssistant.Web.ViewModels.Beehives
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class AllByApiaryIdBeehiveFilterModel : IMapFrom<Beehive>, IMapFrom<BeehiveDataModel>
    {
        [Display(Name = "Сила")]
        public BeehivePower BeehivePower { get; set; }

        [Display(Name = "Система")]
        public BeehiveSystem BeehiveSystem { get; set; }

        [Display(Name = "Вид")]
        public BeehiveType BeehiveType { get; set; }

        [Display(Name = "Номер")]
        public int Number { get; set; }
    }
}
