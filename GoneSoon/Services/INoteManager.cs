using GoneSoon.Models;

namespace GoneSoon.Services
{
    public interface INoteManager
    {
        Task<Note> CreateNewNote(NewNoteDto newNote);
        Task DeleteNote(Guid noteId);
        Task UpdateNote(Note note);
        Task<Note> GetNote(Guid noteId);
    }
}