using GoneSoon.Models;

namespace GoneSoon.Repositories
{
    public interface INoteRepository
    {
        Task<Note> CreateNewNote(Note note);
        Task UpdateNote(Note note);
        Task<Note> GetNote(Guid id);
        Task DeleteNote(Guid id);
    }

}
