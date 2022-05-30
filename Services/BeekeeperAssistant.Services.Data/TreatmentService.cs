﻿namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Filters;
    using BeekeeperAssistant.Data.Filters.Models;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using BeekeeperAssistant.Web.ViewModels.Treatments;
    using Microsoft.EntityFrameworkCore;

    public class TreatmentService : ITreatmentService
    {
        private readonly IDeletableEntityRepository<Beehive> beehiveRepository;
        private readonly IDeletableEntityRepository<Treatment> treatmentRepository;
        private readonly IRepository<TreatedBeehive> treatedBeehivesRepository;

        public TreatmentService(
            IDeletableEntityRepository<Beehive> beehiveRepository,
            IDeletableEntityRepository<Treatment> treatmentRepository,
            IRepository<TreatedBeehive> treatedBeehivesRepository)
        {
            this.beehiveRepository = beehiveRepository;
            this.treatmentRepository = treatmentRepository;
            this.treatedBeehivesRepository = treatedBeehivesRepository;
        }

        public async Task<int> CreateTreatmentAsync(
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
            List<int> beehiveIds)
        {
            var treatment = new Treatment
            {
                CreatorId = creatorId,
                OwnerId = ownerId,
                DateOfTreatment = dateOfTreatment,
                Name = name,
                Note = note,
                Disease = disease,
                Medication = medication,
                InputAs = inputAs,
                Quantity = quantity,
                Dose = dose,
            };

            await this.treatmentRepository.AddAsync(treatment);
            await this.treatmentRepository.SaveChangesAsync();

            foreach (var id in beehiveIds)
            {
                var treatedBeehive = new TreatedBeehive
                {
                    BeehiveId = id,
                    TreatmentId = treatment.Id,
                };

                await this.treatedBeehivesRepository.AddAsync(treatedBeehive);
                await this.treatedBeehivesRepository.SaveChangesAsync();
            }

            return treatment.Id;
        }

        public async Task<int> EditTreatment(
            int treatmentId,
            int beehiveId,
            DateTime dateOfTreatment,
            string name,
            string note,
            string disease,
            string medication,
            InputAs inputAs,
            double quantity,
            Dose dose)
        {
            var treatment = this.treatmentRepository
                .All()
                .FirstOrDefault(t => t.Id == treatmentId);

            treatment.DateOfTreatment = dateOfTreatment;
            treatment.Name = name;
            treatment.Note = note;
            treatment.Disease = disease;
            treatment.Medication = medication;
            treatment.InputAs = inputAs;
            treatment.Quantity = quantity;
            treatment.Dose = dose;

            await this.treatmentRepository.SaveChangesAsync();

            return treatmentId;
        }

        public async Task DeleteTreatmentAsync(int treatmentId)
        {
            var treatment = this.treatmentRepository
                .All()
                .FirstOrDefault(t => t.Id == treatmentId);

            this.treatmentRepository.Delete(treatment);
            await this.treatmentRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllBeehiveTreatments<T>(int beehiveId, int? take = null, int skip = 0, FilterModel filterModel = null)
        {
            var query = this.treatedBeehivesRepository
                .AllAsNoTracking()
                .Where(a => a.Beehive.Id == beehiveId)
                .Select(t => t.Treatment)
                .To<TreatmentDataModel>();

            var filter = new Filter<TreatmentDataModel>();

            query = filter.FilterCollection(query, filterModel);

            query = query.Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public T GetTreatmentById<T>(int treatmentId) =>
            this.treatmentRepository
            .All()
            .Where(t => t.Id == treatmentId)
            .To<T>()
            .FirstOrDefault();

        public int GetAllUserTreatmentsForLastYearCount(string userId) =>
            this.treatmentRepository
                .All()
                .SelectMany(x => x.TreatedBeehives)
                .Where(t => !t.Beehive.IsDeleted &&
                    t.Treatment.CreatorId == userId &&
                    (DateTime.UtcNow.Year - t.Treatment.DateOfTreatment.Year) <= 1)
                .Count();

        public int GetBeehiveTreatmentsCountByBeehiveId(int beehiveId)
            => this.treatedBeehivesRepository
            .All()
            .Where(t => t.BeehiveId == beehiveId && !t.Treatment.IsDeleted)
            .Count();
    }
}
