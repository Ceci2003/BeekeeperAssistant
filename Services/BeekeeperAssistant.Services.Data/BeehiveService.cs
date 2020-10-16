﻿namespace BeekeeperAssistant.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class BeehiveService : IBeehiveService
    {
        private readonly IDeletableEntityRepository<Beehive> beehiveRepository;

        public BeehiveService(IDeletableEntityRepository<Beehive> beehiveRepository)
        {
            this.beehiveRepository = beehiveRepository;
        }

        public IEnumerable<T> GetAllUserBeehivesByApiaryId<T>(int apiaryId)
        {
            var allBeehives = this.beehiveRepository.All().Where(a => a.ApiaryId == apiaryId).To<T>().ToList();
            return allBeehives;
        }
    }
}