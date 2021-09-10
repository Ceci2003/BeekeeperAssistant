namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Web.ViewModels.Inspection;

    public class InspectionService : IInspectionService
    {
        private readonly IDeletableEntityRepository<Inspection> inspectionRepository;

        public InspectionService(IDeletableEntityRepository<Inspection> inspectionRepository)
        {
            this.inspectionRepository = inspectionRepository;
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
                Weight = inputModel.Weight,
                HiveTemperature = inputModel.HiveTemperature,
                HiveHumidity = inputModel.HiveHumidity,
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

            return inspection.Id;
        }

        public IEnumerable<T> GetAllBeehiveInspections<T>(int beehiveId, int? take = null, int skip = 0)
        {
            throw new NotImplementedException();
        }

        public int GetAllUserInspectionsForLastYearCount(string userId) =>
           this.inspectionRepository
               .AllAsNoTracking()
               .Where(t => t.CreatorId == userId)
               .Count();
    }
}
