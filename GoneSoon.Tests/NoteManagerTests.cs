using GoneSoon.InteractionProtocol.NoteService.Data;
using GoneSoon.InteractionProtocol.Services;
using GoneSoon.InteractionProtocol.UserService;
using GoneSoon.InteractionProtocol.UserService.Data;
using GoneSoon.NoteService.Domain;
using GoneSoon.Services;
using Moq;

namespace GoneSoon.Tests
{
    public class NoteManagerTests
    {
        private readonly Mock<INoteServiceClient> _mockNoteServiceClient;
        private readonly Mock<IUserServiceClient> _mockUserService;
        private readonly NoteManager _manager;

        public NoteManagerTests()
        {
            _mockNoteServiceClient = new Mock<INoteServiceClient>();
            _mockUserService = new Mock<IUserServiceClient>();
            _manager = new NoteManager(_mockNoteServiceClient.Object, _mockUserService.Object);
        }

        [Fact]
        public async Task CreateNewNote_CreatesNote()
        {
            var newNoteDto = new NewNoteDto
            {
                Title = "Test Note",
                UserId = 1,
                Content = "content test note",
                DeletingDate = DateTime.Now.AddHours(3),
                NotificationMethods =
                    [
                        new() 
                        {
                            Method = NotificationMethod.Email,
                            Value = "2827365872654875"
                        }
                    ]
            };
            var note = new Note { Id = Guid.NewGuid(), Title = "Test Note" };
            var user = new UserDto { Id = 1 };
            var notificationMethods = new List<NotificationMethodBase> { new EmailNotificationMethod() };

            var createUserRequest = new CreateUserRequest
            {
                DisplayName = "Test User",
                Email = "asdfasdf@ffff.com",
                ExternalId = "1234567890",
                Provider = "TestProvider"
            };

            _mockUserService.Setup(s => s.CreateUserAsync(createUserRequest)).ReturnsAsync(user);
            _mockNoteServiceClient.Setup(s => s.CreateNoteAsync(It.IsAny<NewNoteDto>())).ReturnsAsync(note);

            var result = await _manager.CreateNewNote(newNoteDto);

            Assert.Equal(note, result);
        }

        [Fact]
        public async Task UpdateNote_UpdatesNote()
        {
            var note = new Note { Id = Guid.NewGuid(), Title = "Test Note", Content = "content", ExpireDate = DateTime.Now.AddHours(3) };

            _mockNoteServiceClient.Setup(s => s.GetNoteAsync(note.Id)).ReturnsAsync(note);
            _mockNoteServiceClient.Setup(s => s.UpdateNoteAsync(note)).Returns(Task.CompletedTask);

            await _manager.UpdateNote(note);

            _mockNoteServiceClient.Verify(s => s.UpdateNoteAsync(note), Times.Once);
        }

        [Fact]
        public async Task GetNote_ReturnsNote()
        {
            var noteId = Guid.NewGuid();
            var note = new Note { Id = noteId, Title = "Test Note" };

            _mockNoteServiceClient.Setup(s => s.GetNoteAsync(noteId)).ReturnsAsync(note);

            var result = await _manager.GetNote(noteId);

            Assert.Equal(note, result);
        }

        [Fact]
        public async Task DeleteNote_DeletesNote()
        {
            var noteId = Guid.NewGuid();

            _mockNoteServiceClient.Setup(s => s.DeleteNoteAsync(noteId)).Returns(Task.CompletedTask);

            await _manager.DeleteNote(noteId);

            _mockNoteServiceClient.Verify(s => s.DeleteNoteAsync(noteId), Times.Once);
        }
    }
}
