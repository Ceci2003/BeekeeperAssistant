﻿namespace BeekeeperAssistant.Web.ViewModels.Administration.Beehives
{
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class BeehivesAdministrationViewModel : IMapFrom<Beehive>
    {
        public int Id { get; set; }

        public string ApiaryNumber { get; set; }

        public int Number { get; set; }

        public string CreatorId { get; set; }

        public bool IsDeleted { get; set; }
    }
}