namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class BeehiveHelperService : IBeehiveHelperService
    {
        private readonly IRepository<BeehiveHelper> beehiveHelperRepository;

        public BeehiveHelperService(IRepository<BeehiveHelper> beehiveHelperRepository)
        {
            this.beehiveHelperRepository = beehiveHelperRepository;
        }

        public async Task Add(string userId, int beehiveId)
        {
            var beehiveHelper = new BeehiveHelper()
            {
                Access = Access.Read,
                BeehiveId = beehiveId,
                UserId = userId,
            };

            await this.beehiveHelperRepository.AddAsync(beehiveHelper);
        }

        public Task Delete(string userId, int beehiveId)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(string userId, int beehiveId, Access access)
        {
            var beehiveHelper = this.beehiveHelperRepository.All()
                .FirstOrDefault(x => x.UserId == userId && x.BeehiveId == beehiveId);

            beehiveHelper.Access = access;

            this.beehiveHelperRepository.Update(beehiveHelper);
            await this.beehiveHelperRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllBeehiveHelpersByBeehiveId<T>(int beehiveId, int? take = null, int skip = 0)
        {
            var qurey = this.beehiveHelperRepository
                .AllAsNoTracking()
                .Where(bh => bh.BeehiveId == beehiveId)
                .Skip(skip);

            if (take.HasValue)
            {
                qurey = qurey.Take(take.Value);
            }

            return qurey.To<T>().ToList();
        }

        public T GetBeehiveHelper<T>(string userId, int beehiveId)
        {
            var beehiveHelper = this.beehiveHelperRepository.All()
                .Where(x => x.UserId == userId && x.BeehiveId == beehiveId)
                .To<T>()
                .FirstOrDefault();

            return beehiveHelper;
        }

        public Access GetUserBeehiveAccess(string userId, int beehiveId)
        {
            var access = this.beehiveHelperRepository.All()
                .FirstOrDefault(x => x.BeehiveId == beehiveId && x.UserId == userId);

            return access.Access;
        }

        public bool IsBeehiveHelper(string userId, int beehiveId)
        {
            return this.beehiveHelperRepository.All().Any(x => x.UserId == userId && x.BeehiveId == beehiveId);
        }
    }
}
