using GoneSoon.InteractionProtocol.NoteService.Data;
using System;
using System.Threading.Tasks;

namespace GoneSoon.InteractionProtocol.Services
{
    public interface INoteServiceClient
    {
        Task<Note> CreateNoteAsync(NewNoteDto note);
        Task DeleteNoteAsync(Guid id);
        Task<Note> GetNoteAsync(Guid id);
        Task UpdateNoteAsync(Note note);
    }
}