using BeekeeperAssistant.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeekeeperAssistant.Services.Data
{
    public interface ITreatmentService
    {
        Task<int> CreateTreatment(
            DateTime dateOfTreatment,
            string name,
            string note,
            string disease,
            string medication,
            InputAs inputAs,
            double quantity,
            Dose dose,
            int beehiveId);
    }
}
