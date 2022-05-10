namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BeekeeperAssistant.Data.Filters.Models;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data.Models;

    public interface ITreatmentService
    {
        Task<int> CreateTreatmentAsync(
            string ownerId,
            string creatorId,
            DateTime dateOfTreatment,
            string name,
            string note,
            string disease,
            string medication,
            InputAs inputAs,
            double quantity,
            Dose dose,
            List<int> beehiveIds);

        Task<int> EditTreatment(
            int treatmentId,
            int beehiveId,
            DateTime dateOfTreatment,
            string name,
            string note,
            string disease,
            string medication,
            InputAs inputAs,
            double quantity,
            Dose dose);

        Task DeleteTreatmentAsync(int treatmentId);

        T GetTreatmentById<T>(int treatmentId);

        int GetAllUserTreatmentsForLastYearCount(string userId);

        int GetBeehiveTreatmentsCountByBeehiveId(int beehiveId);

        IEnumerable<T> GetAllBeehiveTreatments<T>(int beehiveId, int? take = null, int skip = 0, FilterModel filterModel = null);
    }
}
