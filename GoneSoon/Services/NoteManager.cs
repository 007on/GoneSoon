using GoneSoon.Models;

namespace GoneSoon.Services
{
    public class NoteManager : INoteManager
    {
        private readonly INoteService _noteService;
        private readonly IUserService _userService;
        private readonly INotificationMethodService _notificationMethodService;

        public NoteManager(INoteService noteService, IUserService userService, INotificationMethodService notificationMethodService)
        {
            _noteService = noteService;
            _userService = userService;
            _notificationMethodService = notificationMethodService;
        }

        public async Task<Note> CreateNewNote(NewNoteDto newNote)
        {
            newNote.ValidateNewNote();
            if (newNote.UserId == 0)
            {
                var user = await _userService.CreateUser();
                newNote.UserId = user.Id;
            }

            (var newNoteModel, var notificationMethods) = newNote.Parse();
            if (notificationMethods.All(x => x.NotificationMethodType == NotificationMethod.None))
            {
                throw new ArgumentException("At least one notification method should be specified");
            }

            var note = await _noteService.CreateNewNoteAsync(newNoteModel);
            notificationMethods.ForEach(x => x.NoteId = note.Id);

            await _notificationMethodService.SaveNotificationMethods(
                [.. notificationMethods.Where(x => x.NotificationMethodType != NotificationMethod.None)]);

            return note;
        }

        public async Task UpdateNote(Note note)
        {
            var existedNote = await _noteService.GetNoteById(note.Id)
                ?? throw new ArgumentException("Note does not exist or was deleted.");

            note.ValidateNote();

            existedNote.Content = note.Content;
            existedNote.ExpireDate = note.ExpireDate;
            existedNote.NotificationMethodTypes = note.NotificationMethodTypes;

            await _noteService.UpdateNote(existedNote);
        }

        public async Task<Note> GetNote(Guid noteId)
        {
            return await _noteService.GetNoteById(noteId);
        }

        public async Task DeleteNote(Guid noteId)
        {
            await _noteService.DeleteNote(noteId);
        }
    }
}
