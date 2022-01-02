namespace BeekeeperAssistant.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IApiaryNoteService
    {
        Task<int> CreateAsync(int apiaryId, string title, string content, string color, string modifiedBy);

        Task<int> EditAsync(int noteId, string title, string content, string color, string modifiedBy);

        T GetApiaryNoteById<T>(int noteid);

        IEnumerable<T> GetAllApiaryNotes<T>(int apiaryId, int? take = null, int skip = 0);

        Task<int> DeleteAsync(int noteId);
    }
}
