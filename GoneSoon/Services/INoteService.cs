using GoneSoon.Models;

namespace GoneSoon.Services
{
    public interface INoteService
    {
        Task<Note> CreateNewNoteAsync(Note newNote);
        Task DeleteNote(Guid noteId);
        Task<Note> GetNoteById(Guid id);
        Task UpdateNote(Note note);
    }
}