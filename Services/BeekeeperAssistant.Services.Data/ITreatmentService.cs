namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;

    public interface ITreatmentService
    {
        Task<int> CreateTreatment(
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

        int GetAllUserTreatmentsForLastYearCount(string userId);
    }
}
