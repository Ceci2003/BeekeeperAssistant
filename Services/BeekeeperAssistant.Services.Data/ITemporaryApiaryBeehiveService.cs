namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ITemporaryApiaryBeehiveService
    {
        T GetApiaryById<T>(int apiaryId);

        Task AddBeehiveToApiary(int apiaryId, int beehiveId);

        Task AddMultipleBeehiveToApiary(int apiaryId, List<int> beehivesIds);

        bool IsBeehiveInTemporary(int beehiveId);

        Task RemoveBeehiveFromTemporaryAsync(int id);

        IEnumerable<T> GetBeehivesByApiaryId<T>(int apiaryId, int? take = null, int skip = 0);
    }
}
