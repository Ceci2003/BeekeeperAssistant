namespace BeekeeperAssistant.Services.Data
{
    using System;
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

        public async Task<int> CreateTreatment(DateTime dateOfTreatment, string name, string note, string disease, string medication, InputAs inputAs, double quantity, Dose dose, int beehiveId)
        {
            var treatment = new Treatment
            {
                DateOfTreatment = dateOfTreatment,
                Name = name,
                Note = note,
                Disease = disease,
                Medication = medication,
                InputAs = inputAs,
                Quantity = quantity,
                Doses = dose,
            };

            await this.treatmentRepository.AddAsync(treatment);
            await this.treatmentRepository.SaveChangesAsync();

            var treatedBeehive = new TreatedBeehive
            {
                BeehiveId = beehiveId,
                TreatmentId = treatment.Id,
            };

            await this.treatedBeehivesRepository.AddAsync(treatedBeehive);
            await this.treatedBeehivesRepository.SaveChangesAsync();

            return treatment.Id;
        }
    }
}
