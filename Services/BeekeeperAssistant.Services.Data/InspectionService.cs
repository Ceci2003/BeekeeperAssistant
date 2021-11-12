namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using BeekeeperAssistant.Web.ViewModels.Inspection;

    public class InspectionService : IInspectionService
    {
        private readonly IDeletableEntityRepository<Inspection> inspectionRepository;
        private readonly IDeletableEntityRepository<Beehive> beehiveRepository;

        public InspectionService(
            IDeletableEntityRepository<Inspection> inspectionRepository,
            IDeletableEntityRepository<Beehive> beehiveRepository)
        {
            this.inspectionRepository = inspectionRepository;
            this.beehiveRepository = beehiveRepository;
        }

        public async Task<int> CreateUserInspectionAsync(string userId, int beehiveId, CreateInspectionInputModel inputModel)
        {
            var inspection = new Inspection
            {
                CreatorId = userId,
                BeehiveId = beehiveId,
                DateOfInspection = inputModel.DateOfInspection,
                InspectionType = inputModel.InspectionType,
                Note = inputModel.Note,
                Swarmed = inputModel.Swarmed,
                BeehivePower = inputModel.BeehivePower,
                BeehiveTemperament = inputModel.BeehiveTemperament,
                BeehiveAction = inputModel.BeehiveAction,
                Weight = inputModel.Weight ?? 0,
                HiveTemperature = inputModel.HiveTemperature ?? 0,
                HiveHumidity = inputModel.HiveHumidity ?? 0,
                IncludeQueenSection = inputModel.IncludeQueenSection,
                QueenSeen = inputModel.QueenSeen,
                QueenCells = inputModel.QueenCells,
                QueenWorkingStatus = inputModel.QueenWorkingStatus,
                QueenPowerStatus = inputModel.QueenPowerStatus,
                IncludeBrood = inputModel.IncludeBrood,
                Eggs = inputModel.Eggs,
                ClappedBrood = inputModel.ClappedBrood,
                UnclappedBrood = inputModel.UnclappedBrood,
                IncludeFramesWith = inputModel.IncludeFramesWith,
                FramesWithBees = inputModel.FramesWithBees,
                FramesWithBrood = inputModel.FramesWithBrood,
                FramesWithHoney = inputModel.FramesWithHoney,
                FramesWithPollen = inputModel.FramesWithPollen,
                IncludeActivity = inputModel.IncludeActivity,
                BeeActivity = inputModel.BeeActivity,
                OrientationActivity = inputModel.OrientationActivity,
                PollenActivity = inputModel.PollenActivity,
                ForragingActivity = inputModel.ForragingActivity,
                BeesPerMinute = inputModel.BeesPerMinute,
                IncludeStorage = inputModel.IncludeStorage,
                StoredHoney = inputModel.StoredHoney,
                StoredPollen = inputModel.StoredPollen,
                IncludeSpottedProblem = inputModel.IncludeSpottedProblem,
                Disease = inputModel.Disease,
                Treatment = inputModel.Treatment,
                Pests = inputModel.Pests,
                Predators = inputModel.Predators,
                IncludeWeatherInfo = inputModel.IncludeWeatherInfo,
                Conditions = inputModel.Conditions,
                WeatherTemperature = inputModel.WeatherTemperature,
                WeatherHumidity = inputModel.WeatherHumidity,
            };

            await this.inspectionRepository.AddAsync(inspection);
            await this.inspectionRepository.SaveChangesAsync();

            // Update beehive power
            var beehive = this.beehiveRepository
                .All()
                .FirstOrDefault(b => b.Id == beehiveId);

            beehive.BeehivePower = inspection.BeehivePower;

            await this.beehiveRepository.SaveChangesAsync();

            return inspection.Id;
        }

        public async Task<int> EditUserInspectionAsync(int inspectionId, EditInspectionInputModel inputModel)
        {
            var inspection = this.inspectionRepository
                .All()
                .FirstOrDefault(i => i.Id == inspectionId);

            inspection.DateOfInspection = inputModel.DateOfInspection;
            inspection.InspectionType = inputModel.InspectionType;
            inspection.Note = inputModel.Note;
            inspection.Swarmed = inputModel.Swarmed;
            inspection.BeehivePower = inputModel.BeehivePower;
            inspection.BeehiveTemperament = inputModel.BeehiveTemperament;
            inspection.BeehiveAction = inputModel.BeehiveAction;
            inspection.Weight = inputModel.Weight ?? 0;
            inspection.HiveTemperature = inputModel.HiveTemperature ?? 0;
            inspection.HiveHumidity = inputModel.HiveHumidity ?? 0;
            inspection.IncludeQueenSection = inputModel.IncludeQueenSection;
            inspection.QueenSeen = inputModel.QueenSeen;
            inspection.QueenCells = inputModel.QueenCells;
            inspection.QueenWorkingStatus = inputModel.QueenWorkingStatus;
            inspection.QueenPowerStatus = inputModel.QueenPowerStatus;
            inspection.IncludeBrood = inputModel.IncludeBrood;
            inspection.Eggs = inputModel.Eggs;
            inspection.ClappedBrood = inputModel.ClappedBrood;
            inspection.UnclappedBrood = inputModel.UnclappedBrood;
            inspection.IncludeFramesWith = inputModel.IncludeFramesWith;
            inspection.FramesWithBees = inputModel.FramesWithBees;
            inspection.FramesWithBrood = inputModel.FramesWithBrood;
            inspection.FramesWithHoney = inputModel.FramesWithHoney;
            inspection.FramesWithPollen = inputModel.FramesWithPollen;
            inspection.IncludeActivity = inputModel.IncludeActivity;
            inspection.BeeActivity = inputModel.BeeActivity;
            inspection.OrientationActivity = inputModel.OrientationActivity;
            inspection.PollenActivity = inputModel.PollenActivity;
            inspection.ForragingActivity = inputModel.ForragingActivity;
            inspection.BeesPerMinute = inputModel.BeesPerMinute;
            inspection.IncludeStorage = inputModel.IncludeStorage;
            inspection.StoredHoney = inputModel.StoredHoney;
            inspection.StoredPollen = inputModel.StoredPollen;
            inspection.IncludeSpottedProblem = inputModel.IncludeSpottedProblem;
            inspection.Disease = inputModel.Disease;
            inspection.Treatment = inputModel.Treatment;
            inspection.Pests = inputModel.Pests;
            inspection.Predators = inputModel.Predators;
            inspection.IncludeWeatherInfo = inputModel.IncludeWeatherInfo;
            inspection.Conditions = inputModel.Conditions;
            inspection.WeatherTemperature = inputModel.WeatherTemperature;
            inspection.WeatherHumidity = inputModel.WeatherHumidity;

            await this.inspectionRepository.SaveChangesAsync();

            return inspection.Id;
        }

        public async Task DeleteInspectionAsync(int inspectionId)
        {
            var inspection = this.inspectionRepository
                .All()
                .FirstOrDefault(i => i.Id == inspectionId);

            this.inspectionRepository.Delete(inspection);
            await this.inspectionRepository.SaveChangesAsync();
        }

        public T GetInspectionById<T>(int inspectionId) =>
            this.inspectionRepository
                .AllAsNoTracking()
                .Where(i => i.Id == inspectionId)
                .To<T>()
                .FirstOrDefault();

        public IEnumerable<T> GetAllBeehiveInspections<T>(int beehiveId, int? take = null, int skip = 0)
        {
            var qurey = this.inspectionRepository
                .AllAsNoTracking()
                .Where(i => i.BeehiveId == beehiveId)
                .OrderByDescending(i => i.DateOfInspection)
                .OrderByDescending(i => i.CreatedOn)
                .Skip(skip);

            if (take.HasValue)
            {
                qurey = qurey.Take(take.Value);
            }

            return qurey.To<T>().ToList();
        }

        public int GetAllUserInspectionsForLastYearCount(string userId) =>
           this.inspectionRepository
               .All()
               .Where(t => !t.Beehive.IsDeleted && t.CreatorId == userId)
               .Count();

        public int GetAllBeehiveInspectionsCountByBeehiveId(int beehiveId)
            => this.inspectionRepository
            .All()
            .Where(i => i.BeehiveId == beehiveId)
            .Count();

        public IEnumerable<Inspection> GetAllInspectionsByBeehiveId(int beehiveId)
            => this.inspectionRepository
            .All()
            .Where(i => i.BeehiveId == beehiveId)
            .ToList();
    }
}
