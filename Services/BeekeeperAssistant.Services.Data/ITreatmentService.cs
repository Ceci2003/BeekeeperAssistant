﻿namespace BeekeeperAssistant.Services.Data
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

        IEnumerable<T> GetAllBeehiveTreatments<T>(int beehiveId, int? take = null, int skip = 0);
    }
}