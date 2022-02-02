namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Expressions;
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
        private readonly ITemporaryApiaryBeehiveService temporaryApiaryBeehiveService;
        private readonly IQueenService queenService;
        private readonly IInspectionService inspectionService;
        private readonly ITreatmentService treatmentService;
        private readonly IHarvestService harvestService;
        private readonly IDeletableEntityRepository<BeehiveDiary> beehiveDiaryRepository;
        private readonly IDeletableEntityRepository<TemporaryApiaryBeehive> temporaryApiaryBeehiveRepository;

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
            IRepository<HarvestedBeehive> harvestedBeehiveRepository,
            ITemporaryApiaryBeehiveService temporaryApiaryBeehiveService,
            IQueenService queenService,
            IInspectionService inspectionService,
            ITreatmentService treatmentService,
            IHarvestService harvestService,
            IDeletableEntityRepository<BeehiveDiary> beehiveDiaryRepository,
            IDeletableEntityRepository<TemporaryApiaryBeehive> temporaryApiaryBeehiveRepository)
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
            this.temporaryApiaryBeehiveService = temporaryApiaryBeehiveService;
            this.queenService = queenService;
            this.inspectionService = inspectionService;
            this.treatmentService = treatmentService;
            this.harvestService = harvestService;
            this.beehiveDiaryRepository = beehiveDiaryRepository;
            this.temporaryApiaryBeehiveRepository = temporaryApiaryBeehiveRepository;
        }

        public async Task<int> CreateBeehiveAsync(
            string ownerId,
            string creatorId,
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
                CreatorId = creatorId,
                OwnerId = ownerId,
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

            if (beehive.Apiary.ApiaryType == ApiaryType.Movable)
            {
                await this.temporaryApiaryBeehiveService.AddBeehiveToApiary(apiaryId, beehive.Id);
            }

            var allApiaryHelpersIds = this.apiaryHelperRepository
                .All()
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
            var beehive = this.beehiveRepository
                .All()
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

        public async Task UpdateBeehiveApiary(int beehiveId, int apiaryId)
        {
            var beehive = this.beehiveRepository
                .All()
                .FirstOrDefault(b => b.Id == beehiveId);

            beehive.ApiaryId = apiaryId;

            await this.beehiveRepository.SaveChangesAsync();
        }

        public async Task UpdateBeehiveNumber(int beehiveId, int beehiveNumber)
        {
            var beehive = this.beehiveRepository
                .All()
                .FirstOrDefault(b => b.Id == beehiveId);

            beehive.Number = beehiveNumber;

            await this.beehiveRepository.SaveChangesAsync();
        }

        public int GetAllBeehivesCountByApiaryId(int apiaryId) =>
            this.beehiveRepository
                .All()
                .Where(b => b.ApiaryId == apiaryId)
                .Count();

        public IEnumerable<T> GetAllUserBeehives<T>(string userId, int? take = null, int skip = 0, string orderBy = null)
        {
            var query = this.beehiveRepository
                .All()
                .Where(b => b.CreatorId == userId && !b.Apiary.IsDeleted);

            if (orderBy != null)
            {
                var parts = orderBy.Split("-");
                query = query.OrderByProeprtyDescending(parts);
            }
            else
            {
                query = query
                .OrderByDescending(b => b.IsBookMarked)
                .ThenBy(b => b.Apiary.Number)
                .ThenBy(b => b.Number);
            }

            query = query.Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            var result = query.To<T>().ToQueryString();
            return query.To<T>().ToList();
        }

        public int GetAllUserBeehivesCount(string userId) =>
            this.beehiveRepository
                .All()
                .Where(b => b.CreatorId == userId && b.Apiary.IsDeleted == false)
                .Count();

        public IEnumerable<T> GetBeehivesByApiaryId<T>(int apiaryId, int? take = null, int skip = 0)
        {
            var query = this.beehiveRepository
                .All()
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

        public IEnumerable<T> GetBeehivesByApiaryIdWithoutInTemporary<T>(int apiaryId) =>
            this.beehiveRepository
                .All()
                .Where(b => b.ApiaryId == apiaryId && !this.temporaryApiaryBeehiveRepository.All().Any(ab => ab.BeehiveId == b.Id))
                .OrderBy(b => b.Number)
                .To<T>()
                .ToList();

        public T GetBeehiveById<T>(int beehiveId) =>
            this.beehiveRepository
                .All()
                .Where(b => b.Id == beehiveId)
                .To<T>()
                .FirstOrDefault();

        public int GetBeehiveIdByQueen(int queenId) =>
            this.queenRepository
                .All()
                .Where(q => q.Id == queenId)
                .FirstOrDefault().BeehiveId;

        public T GetBeehiveByQueenId<T>(int queenId)
        {
            var beehiveId = this.queenRepository
                .All()
                .Where(q => q.Id == queenId)
                .FirstOrDefault().BeehiveId;

            var beehive = this.beehiveRepository
                .All()
                .Where(b => b.Id == beehiveId)
                .To<T>()
                .FirstOrDefault();

            return beehive;
        }

        public T GetBeehiveByNumber<T>(int beehiveNumber, string apiaryNumber)
            => this.beehiveRepository
                .All()
                .Where(b => b.Apiary.Number == apiaryNumber && b.Number == beehiveNumber)
                .To<T>()
                .FirstOrDefault();

        public int AllBeehivesCount()
            => this.beehiveRepository
                .AllWithDeleted()
                .Where(b => !b.Apiary.IsDeleted)
                .Count();

        public async Task<int?> BookmarkBeehiveAsync(int id)
        {
            var beehive = this.beehiveRepository
                .All()
                .FirstOrDefault(b => b.Id == id);

            if (beehive == null)
            {
                return null;
            }

            beehive.IsBookMarked = !beehive.IsBookMarked;

            await this.beehiveRepository.SaveChangesAsync();

            return beehive.ApiaryId;
        }

        public bool BeehiveNumberExistsInApiary(int beehiveNumber, int apiaryId)
        {
            var beehive = this.beehiveRepository
                .All()
                .Where(b => b.ApiaryId == apiaryId && b.Number == beehiveNumber)
                .FirstOrDefault();

            if (beehive == null)
            {
                return false;
            }

            return true;
        }

        public bool BeehiveExistsInApiary(int beehiveId, int apiaryId)
        {
            var beehive = this.beehiveRepository
                .All()
                .Where(b => b.ApiaryId == apiaryId && b.Id == beehiveId)
                .FirstOrDefault();

            if (beehive == null)
            {
                return false;
            }

            return true;
        }

        public IEnumerable<T> GetAllBeehives<T>()
            => this.beehiveRepository
                .All()
                .To<T>()
                .ToList();

        public IEnumerable<T> GetAllBeehivesWithDeleted<T>(int? take = null, int skip = 0, string orderBy = null)
        {
            var query = this.beehiveRepository
                .AllWithDeleted();

            if (orderBy != null)
            {
                query = query.OrderBy(b => b.GetType().GetProperty(orderBy).GetValue(b));
            }

            query = query.Where(b => !b.Apiary.IsDeleted)
                .Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public async Task UndeleteAsync(int beehiveId)
        {
            var beehive = this.beehiveRepository
                .AllWithDeleted()
                .FirstOrDefault(b => b.Id == beehiveId);

            this.beehiveRepository.Undelete(beehive);
            await this.beehiveRepository.SaveChangesAsync();
        }

        public int GetAllBeehivesWithDeletedCount()
        => this.beehiveRepository
                .AllWithDeleted()
                .Where(b => !b.Apiary.IsDeleted)
                .Count();

        public int GetBeehiveNumberById(int id)
        {
            var beehiveNumber = this.beehiveRepository.All().FirstOrDefault(b => b.Id == id).Number;
            return beehiveNumber;
        }

        public int GetBeehiveIdByTreatmentId(int treatmentId)
        {
            var treatment = this.treatedBeehiveRepository.All().FirstOrDefault(t => t.TreatmentId == treatmentId);

            return treatment.BeehiveId;
        }

        public int GetBeehiveIdByHarvesId(int harvestId)
        {
            var harvest = this.harvestedBeehiveRepository.All().FirstOrDefault(h => h.HarvestId == harvestId);

            return harvest.BeehiveId;
        }

        public bool HasDiary(int beehiveId)
        {
            return this.beehiveDiaryRepository.All().Any(bd => bd.BeehiveId == beehiveId);
        }
    }
}
