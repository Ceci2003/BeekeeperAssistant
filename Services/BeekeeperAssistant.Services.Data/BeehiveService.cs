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

    public class BeehiveService : IBeehiveService
    {
        private readonly IRepository<BeehiveHelper> beehiveHelperRepository;
        private readonly IRepository<ApiaryHelper> apiaryHelperRepository;
        private readonly IRepository<QueenHelper> queenHelperRepository;
        private readonly IDeletableEntityRepository<Beehive> beehiveRepository;
        private readonly IDeletableEntityRepository<Queen> queenRepository;
        private readonly IDeletableEntityRepository<Inspection> inspectionRepository;
        private readonly IDeletableEntityRepository<Treatment> treatmentRepository;
        private readonly IRepository<TreatedBeehive> treatedBeehiveRepository;
        private readonly IDeletableEntityRepository<Harvest> harvestRepository;
        private readonly IRepository<HarvestedBeehive> harvestedBeehiveRepository;

        public BeehiveService(
            IRepository<BeehiveHelper> beehiveHelperRepository,
            IRepository<ApiaryHelper> apiaryHelperRepository,
            IRepository<QueenHelper> queenHelperRepository,
            IDeletableEntityRepository<Beehive> beehiveRepository,
            IDeletableEntityRepository<Queen> queenRepository,
            IDeletableEntityRepository<Inspection> inspectionRepository,
            IDeletableEntityRepository<Treatment> treatmentRepository,
            IRepository<TreatedBeehive> treatedBeehiveRepository,
            IDeletableEntityRepository<Harvest> harvestRepository,
            IRepository<HarvestedBeehive> harvestedBeehiveRepository)
        {
            this.beehiveHelperRepository = beehiveHelperRepository;
            this.apiaryHelperRepository = apiaryHelperRepository;
            this.queenHelperRepository = queenHelperRepository;
            this.beehiveRepository = beehiveRepository;
            this.queenRepository = queenRepository;
            this.inspectionRepository = inspectionRepository;
            this.treatmentRepository = treatmentRepository;
            this.treatedBeehiveRepository = treatedBeehiveRepository;
            this.harvestRepository = harvestRepository;
            this.harvestedBeehiveRepository = harvestedBeehiveRepository;
        }

        public async Task<int> CreateUserBeehiveAsync(
            string userId,
            int number,
            BeehiveSystem beehiveSystem,
            BeehiveType beehiveType,
            DateTime dateTime,
            int apiaryId,
            BeehivePower beehivePower,
            bool hasDevice,
            bool hasPolenCatcher,
            bool hasPropolisCatcher,
            bool isItMovable)
        {
            var beehive = new Beehive
            {
                CreatorId = userId,
                Number = number,
                BeehiveSystem = beehiveSystem,
                BeehiveType = beehiveType,
                BeehivePower = beehivePower,
                Date = dateTime,
                ApiaryId = apiaryId,
                HasDevice = hasDevice,
                HasPolenCatcher = hasPolenCatcher,
                HasPropolisCatcher = hasPropolisCatcher,
                IsItMovable = isItMovable,
            };

            await this.beehiveRepository.AddAsync(beehive);
            await this.beehiveRepository.SaveChangesAsync();

            var allApiaryHelpersIds = this.apiaryHelperRepository.All()
                .Where(x => x.ApiaryId == apiaryId)
                .Select(x => x.UserId);

            foreach (var helperId in allApiaryHelpersIds)
            {
                var helper = new BeehiveHelper
                {
                    UserId = helperId,
                    BeehiveId = beehive.Id,
                };

                await this.beehiveHelperRepository.AddAsync(helper);
            }

            await this.beehiveHelperRepository.SaveChangesAsync();

            return beehive.Id;
        }

        public async Task<string> DeleteBeehiveByIdAsync(int beehiveId)
        {
            var queen = this.queenRepository.AllAsNoTracking()
                .FirstOrDefault(q => q.BeehiveId == beehiveId);

            if (queen != null)
            {
                this.queenRepository.Delete(queen);
            }

            var inspections = this.inspectionRepository.All()
                .Where(i => i.BeehiveId == beehiveId)
                .ToList();

            if (inspections.Any())
            {
                foreach (var inspection in inspections)
                {
                    this.inspectionRepository.Delete(inspection);
                }
            }

            var treatments = this.treatedBeehiveRepository.All()
                .Where(tb => tb.BeehiveId == beehiveId)
                .Select(tb => tb.Treatment)
                .ToList();

            if (treatments.Any())
            {
                foreach (var treatment in treatments)
                {
                    this.treatmentRepository.Delete(treatment);
                }
            }

            var harvests = this.harvestedBeehiveRepository.All()
                .Where(hb => hb.BeehiveId == beehiveId)
                .Select(hb => hb.Harvest)
                .ToList();

            if (harvests.Any())
            {
                foreach (var harvest in harvests)
                {
                    this.harvestRepository.Delete(harvest);
                }
            }

            var allBeehiveHelpersToDelete = this.beehiveHelperRepository.All()
                .Where(x => x.BeehiveId == beehiveId);

            foreach (var beehiveHelper in allBeehiveHelpersToDelete)
            {
                this.beehiveHelperRepository.Delete(beehiveHelper);
            }

            await this.beehiveHelperRepository.SaveChangesAsync();

            if (queen != null)
            {
                var allQueenHelpersToDelete = this.queenHelperRepository.All()
                .Where(x => x.QueenId == queen.Id);

                foreach (var queenHelper in allQueenHelpersToDelete)
                {
                    this.queenHelperRepository.Delete(queenHelper);
                }

                await this.queenHelperRepository.SaveChangesAsync();
            }

            var beehive = this.beehiveRepository
                .All()
                .Include(b => b.Apiary)
                .FirstOrDefault(b => b.Id == beehiveId);

            this.beehiveRepository.Delete(beehive);
            await this.beehiveRepository.SaveChangesAsync();

            return beehive.Apiary.Number;
        }

        public async Task<int> EditUserBeehiveAsync(
            int beehiveId,
            int number,
            BeehiveSystem beehiveSystem,
            BeehiveType beehiveType,
            DateTime dateTime,
            int apiaryId,
            BeehivePower beehivePower,
            bool hasDevice,
            bool hasPolenCatcher,
            bool hasPropolisCatcher,
            bool isItMovable)
        {
            var beehive = this.beehiveRepository
                .All()
                .FirstOrDefault(b => b.Id == beehiveId);

            beehive.Number = number;
            beehive.BeehiveSystem = beehiveSystem;
            beehive.BeehiveType = beehiveType;
            beehive.BeehivePower = beehivePower;
            beehive.Date = dateTime;
            beehive.ApiaryId = apiaryId;
            beehive.HasDevice = hasDevice;
            beehive.HasPolenCatcher = hasPolenCatcher;
            beehive.HasPropolisCatcher = hasPropolisCatcher;
            beehive.IsItMovable = isItMovable;

            await this.beehiveRepository.SaveChangesAsync();

            return beehive.Id;
        }

        public int GetAllBeehivesCountByApiaryId(int apiaryId) =>
            this.beehiveRepository
                .AllAsNoTracking()
                .Where(b => b.ApiaryId == apiaryId)
                .Count();

        public IEnumerable<T> GetAllUserBeehives<T>(string userId, int? take = null, int skip = 0)
        {
            var query = this.beehiveRepository
                .AllAsNoTracking()
                .OrderByDescending(b => b.IsBookMarked)
                .ThenBy(b => b.Apiary.Number)
                .ThenBy(b => b.Number)
                .Where(b => b.CreatorId == userId && b.Apiary.IsDeleted == false)
                .Skip(skip);

            var queryAsList = query.ToList();

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            var result = query.To<T>().ToList();
            return result;
        }

        public int GetAllUserBeehivesCount(string userId) =>
            this.beehiveRepository
                .AllAsNoTracking()
                .Where(b => b.CreatorId == userId && b.Apiary.IsDeleted == false)
                .Count();

        public IEnumerable<T> GetBeehivesByApiaryId<T>(int apiaryId, int? take = null, int skip = 0)
        {
            var query = this.beehiveRepository.All()
                .OrderByDescending(b => b.IsBookMarked)
                .ThenBy(b => b.Number)
                .Where(b => b.ApiaryId == apiaryId)
                .Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public T GetBeehiveById<T>(int beehiveId) =>
            this.beehiveRepository.AllAsNoTracking()
                .Include(x => x.Queen.User)
                .Where(b => b.Id == beehiveId)
                .To<T>()
                .FirstOrDefault();

        public int GetBeehiveIdByQueen(int queenId) =>
            this.queenRepository.AllAsNoTracking()
                .Include(x => x.User)
                .Where(q => q.Id == queenId)
                .FirstOrDefault().BeehiveId;

        public T GetBeehiveByQueen<T>(int queenId)
        {
            var id = this.queenRepository.AllAsNoTracking()
                .Include(x => x.User)
                .Where(q => q.Id == queenId)
                .FirstOrDefault().BeehiveId;

            var beehive = this.beehiveRepository.AllAsNoTracking()
                .Where(b => b.Id == id)
                .To<T>()
                .FirstOrDefault();

            return beehive;
        }

        public T GetBeehiveByNumber<T>(int beehiveNumber, string apiaryNumber) =>
            this.beehiveRepository.AllAsNoTracking()
                .Where(b => b.Apiary.Number == apiaryNumber && b.Number == beehiveNumber)
                .To<T>()
                .FirstOrDefault();

        public async Task<int?> BookmarkBeehiveAsync(int id)
        {
            var beehive = this.beehiveRepository.All().FirstOrDefault(b => b.Id == id);

            if (beehive == null)
            {
                return null;
            }

            beehive.IsBookMarked = !beehive.IsBookMarked;

            await this.beehiveRepository.SaveChangesAsync();

            return beehive.ApiaryId;
        }
    }
}
