using GoneSoon.Models;
using GoneSoon.Repositories;

namespace GoneSoon.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;

        public NoteService(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public async Task<Note> CreateNewNoteAsync(Note newNote)
        {
            newNote.Id = Guid.NewGuid();
            return await _noteRepository.CreateNewNote(newNote);
        }

        public async Task UpdateNote(Note note)
        {
            _ = await _noteRepository.GetNote(note.Id)
                ?? throw new ArgumentException("Note does not exist or was deleted.");

            await _noteRepository.UpdateNote(note);
        }

        public async Task DeleteNote(Guid noteId)
        {
            await _noteRepository.DeleteNote(noteId);
        }

        public async Task<Note> GetNoteById(Guid id)
        {
            return await _noteRepository.GetNote(id);
        }
    }
}
