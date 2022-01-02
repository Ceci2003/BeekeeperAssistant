namespace BeekeeperAssistant.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class BeehiveNoteService : IBeehiveNoteService
    {
        private readonly IDeletableEntityRepository<BeehiveNote> beehiveNotesRepository;

        public BeehiveNoteService(
            IDeletableEntityRepository<BeehiveNote> beehiveNotesRepository)
        {
            this.beehiveNotesRepository = beehiveNotesRepository;
        }

        public async Task<int> CreateAsync(int beehiveId, string title, string content, string color, string modifiedBy)
        {
            var note = new BeehiveNote
            {
                BeehiveId = beehiveId,
                ModifiendById = modifiedBy,
                Title = title,
                Content = content,
                Color = color,
            };

            await this.beehiveNotesRepository.AddAsync(note);
            await this.beehiveNotesRepository.SaveChangesAsync();

            note.ModifiedOn = note.CreatedOn;
            await this.beehiveNotesRepository.SaveChangesAsync();

            return note.Id;
        }

        public async Task<int> DeleteAsync(int noteId)
        {
            var note = this.beehiveNotesRepository
                .All()
                .FirstOrDefault(n => n.Id == noteId);

            this.beehiveNotesRepository.Delete(note);
            await this.beehiveNotesRepository.SaveChangesAsync();

            return note.BeehiveId;
        }

        public async Task<int> EditAsync(int noteId, string title, string content, string color, string modifiedBy)
        {
            var note = this.beehiveNotesRepository
                .All()
                .FirstOrDefault(n => n.Id == noteId);

            note.Title = title;
            note.Content = content;
            note.Color = color;
            note.ModifiendById = modifiedBy;

            await this.beehiveNotesRepository.SaveChangesAsync();

            return note.BeehiveId;
        }

        public T GetBeehiveNoteById<T>(int noteid)
        {
            var note = this.beehiveNotesRepository
                .All()
                .Where(n => n.Id == noteid)
                .To<T>()
                .FirstOrDefault();

            return note;
        }

        public IEnumerable<T> GetAllBeehiveNotes<T>(int beehiveId, int? take = null, int skip = 0)
        {
            var query = this.beehiveNotesRepository
                .All()
                .OrderByDescending(n => n.ModifiedOn)
                .Where(n => n.BeehiveId == beehiveId)
                .Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }
    }
}
