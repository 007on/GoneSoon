using GoneSoon.Models;
using GoneSoon.Services;
using Moq;

namespace GoneSoon.Tests
{
    public class NoteManagerTests
    {
        private readonly Mock<INoteService> _mockNoteService;
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<INotificationMethodService> _mockNotificationMethodService;
        private readonly NoteManager _manager;

        public NoteManagerTests()
        {
            _mockNoteService = new Mock<INoteService>();
            _mockUserService = new Mock<IUserService>();
            _mockNotificationMethodService = new Mock<INotificationMethodService>();
            _manager = new NoteManager(_mockNoteService.Object, _mockUserService.Object, _mockNotificationMethodService.Object);
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
                NotificationMethods = new List<NotificationRequestDto>
                    {
                        new NotificationRequestDto
                        {
                            Method = NotificationMethod.Email,
                            Value = "2827365872654875"
                        }
                    }
            };
            var note = new Note { Id = Guid.NewGuid(), Title = "Test Note" };
            var user = new User { Id = 1 };
            var notificationMethods = new List<NotificationMethodBase> { new EmailNotificationMethod() };

            _mockUserService.Setup(s => s.CreateUser()).ReturnsAsync(user);
            _mockNoteService.Setup(s => s.CreateNewNoteAsync(It.IsAny<Note>())).ReturnsAsync(note);
            _mockNotificationMethodService.Setup(s => s.SaveNotificationMethods(It.IsAny<List<NotificationMethodBase>>())).Returns(Task.CompletedTask);

            var result = await _manager.CreateNewNote(newNoteDto);

            Assert.Equal(note, result);
        }

        [Fact]
        public async Task CreateNewNote_CreatesNote_EmptyContentError()
        {
            var newNoteDto = new NewNoteDto
            {
                Title = "Test Note",
                UserId = 1,
                Content = string.Empty,
                DeletingDate = DateTime.Now.AddHours(3),
                NotificationMethods = new List<NotificationRequestDto>
                    {
                        new NotificationRequestDto
                        {
                            Method = NotificationMethod.Email,
                            Value = "2827365872654875"
                        }
                    }
            };
            var note = new Note { Id = Guid.NewGuid(), Title = "Test Note" };
            var user = new User { Id = 1 };
            var notificationMethods = new List<NotificationMethodBase> { new EmailNotificationMethod() };

            _mockNotificationMethodService.Setup(s => s.SaveNotificationMethods(It.IsAny<List<NotificationMethodBase>>())).Returns(Task.CompletedTask);

            await Assert.ThrowsAsync<ArgumentException>(() => _manager.CreateNewNote(newNoteDto));

            _mockNoteService.Verify(s => s.CreateNewNoteAsync(It.IsAny<Note>()), Times.Never);
            _mockNotificationMethodService.Verify(s => s.SaveNotificationMethods(It.IsAny<List<NotificationMethodBase>>()), Times.Never);
        }

        [Fact]
        public async Task CreateNewNote_CreatesNote_EmptyTitleError()
        {
            var newNoteDto = new NewNoteDto
            {
                Title = string.Empty,
                UserId = 1,
                Content = "Test",
                DeletingDate = DateTime.Now.AddHours(3),
                NotificationMethods = new List<NotificationRequestDto>
                    {
                        new NotificationRequestDto
                        {
                            Method = NotificationMethod.Email,
                            Value = "2827365872654875"
                        }
                    }
            };
            var note = new Note { Id = Guid.NewGuid(), Title = "Test Note" };
            var user = new User { Id = 1 };
            var notificationMethods = new List<NotificationMethodBase> { new EmailNotificationMethod() };

            _mockNotificationMethodService.Setup(s => s.SaveNotificationMethods(It.IsAny<List<NotificationMethodBase>>())).Returns(Task.CompletedTask);

            await Assert.ThrowsAsync<ArgumentException>(() => _manager.CreateNewNote(newNoteDto));

            _mockNoteService.Verify(s => s.CreateNewNoteAsync(It.IsAny<Note>()), Times.Never);
            _mockNotificationMethodService.Verify(s => s.SaveNotificationMethods(It.IsAny<List<NotificationMethodBase>>()), Times.Never);
        }

        [Fact]
        public async Task CreateNewNote_CreatesNote_EmptyNotificationMethodsError()
        {
            var newNoteDto = new NewNoteDto
            {
                Title = "Test Note",
                UserId = 1,
                Content = "Test",
                DeletingDate = DateTime.Now.AddHours(3)
            };
            var note = new Note { Id = Guid.NewGuid(), Title = "Test Note" };
            var user = new User { Id = 1 };
            var notificationMethods = new List<NotificationMethodBase> { new EmailNotificationMethod() };

            _mockNotificationMethodService.Setup(s => s.SaveNotificationMethods(It.IsAny<List<NotificationMethodBase>>())).Returns(Task.CompletedTask);

            await Assert.ThrowsAsync<ArgumentException>(() => _manager.CreateNewNote(newNoteDto));

            _mockNoteService.Verify(s => s.CreateNewNoteAsync(It.IsAny<Note>()), Times.Never);
            _mockNotificationMethodService.Verify(s => s.SaveNotificationMethods(It.IsAny<List<NotificationMethodBase>>()), Times.Never);
        }

        [Fact]
        public async Task CreateNewNote_CreatesNote_InvalidtDeletingTimeError()
        {
            var newNoteDto = new NewNoteDto
            {
                Title = "Test Note",
                UserId = 1,
                Content = "Test",
                DeletingDate = DateTime.Now.AddHours(-3),
                NotificationMethods = new List<NotificationRequestDto>
                    {
                        new NotificationRequestDto
                        {
                            Method = NotificationMethod.Email,
                            Value = "2827365872654875"
                        }
                    }
            };
            var note = new Note { Id = Guid.NewGuid(), Title = "Test Note" };
            var user = new User { Id = 1 };
            var notificationMethods = new List<NotificationMethodBase> { new EmailNotificationMethod() };

            _mockNotificationMethodService.Setup(s => s.SaveNotificationMethods(It.IsAny<List<NotificationMethodBase>>())).Returns(Task.CompletedTask);

            await Assert.ThrowsAsync<ArgumentException>(() => _manager.CreateNewNote(newNoteDto));

            _mockNoteService.Verify(s => s.CreateNewNoteAsync(It.IsAny<Note>()), Times.Never);
            _mockNotificationMethodService.Verify(s => s.SaveNotificationMethods(It.IsAny<List<NotificationMethodBase>>()), Times.Never);
        }

        [Fact]
        public async Task UpdateNote_UpdatesNote()
        {
            var note = new Note { Id = Guid.NewGuid(), Title = "Test Note", Content = "content", ExpireDate = DateTime.Now.AddHours(3) };

            _mockNoteService.Setup(s => s.GetNoteById(note.Id)).ReturnsAsync(note);
            _mockNoteService.Setup(s => s.UpdateNote(note)).Returns(Task.CompletedTask);

            await _manager.UpdateNote(note);

            _mockNoteService.Verify(s => s.UpdateNote(note), Times.Once);
        }

        [Fact]
        public async Task UpdateNote_EmptyTitleError()
        {
            var note = new Note { Id = Guid.NewGuid(), Title = string.Empty, Content = "content", ExpireDate = DateTime.Now.AddHours(3) };

            _mockNoteService.Setup(s => s.GetNoteById(note.Id)).ReturnsAsync(note);

            await Assert.ThrowsAsync<ArgumentException>(() => _manager.UpdateNote(note));

            _mockNoteService.Verify(s => s.UpdateNote(It.IsAny<Note>()), Times.Never);
        }

        [Fact]
        public async Task UpdateNote_EmptyContentError()
        {
            var note = new Note { Id = Guid.NewGuid(), Title = "Test Note", Content = string.Empty, ExpireDate = DateTime.Now.AddHours(3) };

            _mockNoteService.Setup(s => s.GetNoteById(note.Id)).ReturnsAsync(note);

            await Assert.ThrowsAsync<ArgumentException>(() => _manager.UpdateNote(note));

            _mockNoteService.Verify(s => s.UpdateNote(It.IsAny<Note>()), Times.Never);
        }

        [Fact]
        public async Task UpdateNote_InvalidExpireDateError()
        {
            var note = new Note { Id = Guid.NewGuid(), Title = "Test Note", Content = "content", ExpireDate = DateTime.Now.AddHours(-3) };

            _mockNoteService.Setup(s => s.GetNoteById(note.Id)).ReturnsAsync(note);

            await Assert.ThrowsAsync<ArgumentException>(() => _manager.UpdateNote(note));

            _mockNoteService.Verify(s => s.UpdateNote(It.IsAny<Note>()), Times.Never);
        }

        [Fact]
        public async Task GetNote_ReturnsNote()
        {
            var noteId = Guid.NewGuid();
            var note = new Note { Id = noteId, Title = "Test Note" };

            _mockNoteService.Setup(s => s.GetNoteById(noteId)).ReturnsAsync(note);

            var result = await _manager.GetNote(noteId);

            Assert.Equal(note, result);
        }

        [Fact]
        public async Task DeleteNote_DeletesNote()
        {
            var noteId = Guid.NewGuid();

            _mockNoteService.Setup(s => s.DeleteNote(noteId)).Returns(Task.CompletedTask);

            await _manager.DeleteNote(noteId);

            _mockNoteService.Verify(s => s.DeleteNote(noteId), Times.Once);
        }
    }
}
