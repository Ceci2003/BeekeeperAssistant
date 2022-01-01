namespace BeekeeperAssistant.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class ApiaryNoteService : IApiaryNoteService
    {
        private readonly IDeletableEntityRepository<ApiaryNote> apiaryNotesRepository;

        public ApiaryNoteService(
            IDeletableEntityRepository<ApiaryNote> apiaryNotesRepository)
        {
            this.apiaryNotesRepository = apiaryNotesRepository;
        }

        public async Task<int> CreateAsync(int apiaryId, string title, string content, string color, string modifiedBy)
        {
            var note = new ApiaryNote
            {
                ApiaryId = apiaryId,
                ModifiendById = modifiedBy,
                Title = title,
                Content = content,
                Color = color,
            };

            await this.apiaryNotesRepository.AddAsync(note);
            await this.apiaryNotesRepository.SaveChangesAsync();

            note.ModifiedOn = note.CreatedOn;
            await this.apiaryNotesRepository.SaveChangesAsync();

            return note.Id;
        }

        public async Task<int> DeleteAsync(int noteId)
        {
            var note = this.apiaryNotesRepository
                .All()
                .FirstOrDefault(n => n.Id == noteId);

            this.apiaryNotesRepository.Delete(note);
            await this.apiaryNotesRepository.SaveChangesAsync();

            return note.ApiaryId;
        }

        public async Task<int> EditAsync(int noteId, string title, string content, string color, string modifiedBy)
        {
            var note = this.apiaryNotesRepository
                .All()
                .FirstOrDefault(n => n.Id == noteId);

            note.Title = title;
            note.Content = content;
            note.Color = color;
            note.ModifiendById = modifiedBy;

            await this.apiaryNotesRepository.SaveChangesAsync();

            return note.ApiaryId;
        }

        public T GetApiaryNoteById<T>(int noteid)
        {
            var note = this.apiaryNotesRepository
                .All()
                .Where(n => n.Id == noteid)
                .To<T>()
                .FirstOrDefault();

            return note;
        }

        public IEnumerable<T> GetAllApiaryNotes<T>(int apiaryId, int? take = null, int skip = 0)
        {
            var query = this.apiaryNotesRepository
                .All()
                .OrderByDescending(n => n.ModifiedOn)
                .Where(n => n.ApiaryId == apiaryId)
                .Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }
    }
}
