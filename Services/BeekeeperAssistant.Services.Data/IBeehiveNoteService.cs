namespace BeekeeperAssistant.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBeehiveNoteService
    {
        Task<int> CreateAsync(int beehiveId, string title, string content, string color, string modifiedBy);

        Task<int> EditAsync(int noteId, string title, string content, string color, string modifiedBy);

        T GetBeehiveNoteById<T>(int noteid);

        IEnumerable<T> GetAllBeehiveNotes<T>(int beehiveId, int? take = null, int skip = 0);

        Task<int> DeleteAsync(int noteId);
    }
}
