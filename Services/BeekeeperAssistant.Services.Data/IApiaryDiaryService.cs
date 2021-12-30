using BeekeeperAssistant.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeekeeperAssistant.Services.Data
{
    public interface IApiaryDiaryService
    {
        Task<int> CreateAsync(int apiaryId, string content, string modifiedBy);

        T GetApiaryDiaryByApiaryId<T>(int apiaryId);

        Task<int> SaveAsync(int apiaryId, string content, string modifiedBy);
    }
}
