using GoneSoon.InteractionProtocol.NoteService.Data;
using GoneSoon.InteractionProtocol.Services;
using GoneSoon.InteractionProtocol.UserService;

namespace GoneSoon.Services
{
    public class NoteManager : INoteManager
    {
        private readonly IUserServiceClient _userServiceClient;
        private readonly INoteServiceClient _noteServiceClient;

        public NoteManager(INoteServiceClient noteServiceClient, IUserServiceClient userServiceClient)
        {
            _userServiceClient = userServiceClient;
            _noteServiceClient = noteServiceClient;
        }

        public async Task<Note> CreateNewNote(NewNoteDto newNote)
        {
            if (newNote.UserId == 0)
            {
                //var user = await _userServiceClient.CreateUserAsync();
                //newNote.UserId = user.Id;
            }

            var note = await _noteServiceClient.CreateNoteAsync(newNote);

            return note;
        }

        public async Task UpdateNote(Note note)
        {
            await _noteServiceClient.UpdateNoteAsync(note);
        }

        public async Task<Note> GetNote(Guid noteId)
        {
            return await _noteServiceClient.GetNoteAsync(noteId);
        }

        public async Task DeleteNote(Guid noteId)
        {
            await _noteServiceClient.DeleteNoteAsync(noteId);
        }
    }
}
