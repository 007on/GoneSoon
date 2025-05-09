using GoneSoon.NoteService.Domain;
using GoneSoon.NoteService.Helpers;
using GoneSoon.NoteService.Repositories;

namespace GoneSoon.NoteService.Services
{
    public class NoteService(INoteRepository noteRepository, INotificationMethodRepository notificationMethodRepository) : INoteService
    {
        private readonly INoteRepository _noteRepository = noteRepository;
        private readonly INotificationMethodRepository _notificationMethodRepository = notificationMethodRepository;

        public async Task<Note> CreateNewNote(NewNoteDto newNote)
        {
            newNote.ValidateNewNote();

            (var newNoteModel, var notificationMethods) = newNote.Parse();
            if (notificationMethods.All(x => x.NotificationMethodType == NotificationMethod.None))
            {
                throw new ArgumentException("At least one notification method should be specified");
            }

            var note = await _noteRepository.CreateNewNote(newNoteModel);
            notificationMethods.ForEach(x => x.NoteId = note.Id);

            await _notificationMethodRepository.SaveNotificationMethods(
                [.. notificationMethods.Where(x => x.NotificationMethodType != NotificationMethod.None)]);

            return note;
        }

        public async Task UpdateNote(Note note)
        {
            var existedNote = await _noteRepository.GetNote(note.Id)
                ?? throw new ArgumentException("Note does not exist or was deleted.");

            note.ValidateNote();

            existedNote.Content = note.Content;
            existedNote.ExpireDate = note.ExpireDate;
            existedNote.NotificationMethodTypes = note.NotificationMethodTypes;

            await _noteRepository.UpdateNote(existedNote);
        }

        public async Task<Note> GetNote(Guid noteId)
        {
            return await _noteRepository.GetNote(noteId);
        }

        public async Task DeleteNote(Guid noteId)
        {
            await _noteRepository.DeleteNote(noteId);
        }
    }
}
