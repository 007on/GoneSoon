using GoneSoon.InteractionProtocol.NoteService.Data;

namespace GoneSoon.Services
{
    public interface INoteManager
    {
        Task<Note> CreateNewNote(NewNoteDto newNote);
        Task DeleteNote(Guid noteId);
        Task<Note> GetNote(Guid noteId);
        Task UpdateNote(Note note);
    }
}
