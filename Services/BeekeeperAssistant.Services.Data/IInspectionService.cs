namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Web.ViewModels.Inspection;

    public interface IInspectionService
    {
        //Task<int> CreateUserInspectionAsync(
        //    string userId,
        //    int beehiveId,
        //    DateTime dateOfInspection,
        //    InspectionType inspectionType,
        //    string note,
        //    bool swarmed,
        //    BeehivePower beehivePower,
        //    BeehiveTemperament beehiveTemperament,
        //    BeehiveAction beehiveActions,
        //    double weight,
        //    double hiveTemperature,
        //    double hiveHumidity,
        //    bool includeQueenSection,
        //    bool queenSeen,
        //    QueenCells queenCells,
        //    QueenWorkingStatus queenWorkingStatus,
        //    QueenPowerStatus queenPowerStatus,
        //    bool includeBrood,
        //    bool eggs,
        //    bool clappedBrood,
        //    bool unclappedBrood,
        //    bool includeFramesWith,
        //    int framesWithBees,
        //    int framesWithBrood,
        //    int framesWithHoney,
        //    int framesWithPollen,
        //    bool includeActivity,
        //    Activity beeActivity,
        //    Activity orientationActivity,
        //    Activity pollenActivity,
        //    Activity forragingActivity,
        //    int beesPerMinute,
        //    bool includeStorage,
        //    StoragePower storedHoney,
        //    StoragePower storedPollen,
        //    bool includeSpottedProblem,
        //    string disease,
        //    string treatment,
        //    string pests,
        //    string predators,
        //    bool includeWeatherInfo,
        //    string conditions,
        //    double weatherTemperature,
        //    double weatherHumidity);

        Task<int> CreateUserInspectionAsync(
            string userId,
            int beehiveId,
            CreateInspectionInputModel inputModel);

        Task<int> EditUserInspectionAsync(
            int inspectionId,
            EditInspectionInputModel inputModel);

        Task DeleteInspectionAsync(int inspectionId);

        T GetInspectionById<T>(int inspectionId);

        int GetAllUserInspectionsForLastYearCount(string userId);

        IEnumerable<T> GetAllBeehiveInspections<T>(int beehiveId, int? take = null, int skip = 0);
    }
}
