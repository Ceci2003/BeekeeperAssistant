namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using Microsoft.AspNetCore.Identity;

    public class QueenHelperService : IQueenHelperService
    {
        private readonly IRepository<QueenHelper> queenHelperRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRepository<Queen> queenRepository;

        public QueenHelperService(
            IRepository<QueenHelper> queenHelperRepository,
            UserManager<ApplicationUser> userManager,
            IRepository<Queen> queenRepository)
        {
            this.queenHelperRepository = queenHelperRepository;
            this.userManager = userManager;
            this.queenRepository = queenRepository;
        }

        public async Task EditAsync(string userId, int queenId, Access access)
        {
            var queenHelper = this.queenHelperRepository
                .All()
                .FirstOrDefault(x => x.UserId == userId && x.QueenId == queenId);

            queenHelper.Access = access;

            this.queenHelperRepository.Update(queenHelper);
            await this.queenHelperRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllQueenByQueenId<T>(int queenId, int? take = null, int skip = 0)
        {
            var qurey = this.queenHelperRepository
                .All()
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
            var queenHelper = this.queenHelperRepository
                .All()
                .Where(x => x.UserId == userId && x.QueenId == queenId)
                .To<T>()
                .FirstOrDefault();

            return queenHelper;
        }

        public async Task<Access> GetUserQueenAccessAsync(string userId, int queenId)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            var apiaryCreatorId = this.queenRepository.All()
                .Where(q => q.Id == queenId)
                .Select(q => q.Beehive.Apiary.CreatorId)
                .FirstOrDefault();

            if (await this.userManager.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName) || user.Id == apiaryCreatorId)
            {
                return Access.ReadWrite;
            }

            return this.queenHelperRepository.All().FirstOrDefault(qh => qh.UserId == userId && qh.QueenId == queenId).Access;
        }
    }
}
