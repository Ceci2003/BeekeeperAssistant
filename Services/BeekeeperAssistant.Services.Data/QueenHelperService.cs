﻿namespace BeekeeperAssistant.Services.Data
{
    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BeekeeperAssistant.Services.Mapping;
    using System.Threading.Tasks;

    public class QueenHelperService : IQueenHelperService
    {
        private readonly IRepository<QueenHelper> queenHelperRepository;

        public QueenHelperService(IRepository<QueenHelper> queenHelperRepository)
        {
            this.queenHelperRepository = queenHelperRepository;
        }

        public async Task Edit(string userId, int queenId, Access access)
        {
            var queenHelper = this.queenHelperRepository.All()
                .FirstOrDefault(x => x.UserId == userId && x.QueenId == queenId);

            queenHelper.Access = access;

            this.queenHelperRepository.Update(queenHelper);
            await this.queenHelperRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllQueenByQueenId<T>(int queenId, int? take = null, int skip = 0)
        {
            var qurey = this.queenHelperRepository
                .AllAsNoTracking()
                .Where(bh => bh.QueenId == queenId)
                .Skip(skip);

            if (take.HasValue)
            {
                qurey = qurey.Take(take.Value);
            }

            return qurey.To<T>().ToList();
        }

        public T GetQueenHelper<T>(string userId, int queenId)
        {
            var queenHelper = this.queenHelperRepository.All()
                .Where(x => x.UserId == userId && x.QueenId == queenId)
                .To<T>()
                .FirstOrDefault();

            return queenHelper;
        }
    }
}
