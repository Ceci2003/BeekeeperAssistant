namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;

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

        public async Task<int> CreateTreatment(string creatorId, DateTime dateOfTreatment, string name, string note, string disease, string medication, InputAs inputAs, double quantity, Dose dose, List<int> beehiveIds)
        {
            var treatment = new Treatment
            {
                CreatorId = creatorId,
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

        public int GetAllUserTreatmentsForLastYearCount(string userId) =>
           this.treatmentRepository
               .AllAsNoTracking()
               .Where(t => t.CreatorId == userId && (DateTime.UtcNow.Year - t.DateOfTreatment.Year) <= 1)
               .Count();
    }
}
