using GoneSoon.InteractionProtocol;
using GoneSoon.NoteService.Domain;

namespace GoneSoon.NoteService.Repositories
{
    public interface INoteRepository
    {
        Task<Note> CreateNewNote(Note note);
        Task UpdateNote(Note note);
        Task<Note> GetNote(Guid id);
        Task DeleteNote(Guid id);
        Task<NoteMetadata> GetNoteMetadata(Guid id);
    }
}
