using GoneSoon.InteractionProtocol.NoteService.Data;
using GoneSoon.NoteService.Repositories;
using Moq;

namespace GoneSoon.Tests
{
    public class NoteServiceTests
    {
        private readonly Mock<INoteRepository> _mockRepository;
        private readonly Mock<INotificationMethodRepository> _mockNotificationMethodRepository;
        private readonly NoteService.Services.NoteService _noteService;

        public NoteServiceTests()
        {
            _mockRepository = new Mock<INoteRepository>();
            _mockNotificationMethodRepository = new Mock<INotificationMethodRepository>();
            _noteService = new NoteService.Services.NoteService(_mockRepository.Object, _mockNotificationMethodRepository.Object);
        }

        [Fact]
        public async Task CreateNewNote_CreatesNote()
        {

            var newNoteDto = new NewNoteDto
            {
                Title = "Test Note",
                UserId = 1,
                Content = "Test content",
                DeletingDate = DateTime.Now.AddHours(3),
                NotificationMethods = new List<NotificationRequestDto>
                    {
                        new NotificationRequestDto
                        {
                            Method = NotificationMethod.Email,
                            Value = "2827365872654875"
                        }
                    }
            }; var note = new Note { Id = Guid.NewGuid(), Title = "Test Note" };

            _mockRepository.Setup(r => r.CreateNewNote(It.IsAny<Note>())).ReturnsAsync(note);

            var result = await _noteService.CreateNewNote(newNoteDto);

            Assert.Equal(note, result);
        }

        [Fact]
        public async Task CreateNewNote_ThrowsException_WhenNoContent()
        {

            var newNoteDto = new NewNoteDto
            {
                Title = "Test Note",
                UserId = 1,
                Content = string.Empty, // Empty content
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

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _noteService.CreateNewNote(newNoteDto));
            Assert.Equal("Content is required", exception.Message);
        }

        [Fact]
        public async Task CreateNewNote_ThrowsException_WhenNoUserId()
        {

            var newNoteDto = new NewNoteDto
            {
                Title = "Test Note",
                UserId = 0,
                Content = "content",
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

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _noteService.CreateNewNote(newNoteDto));
            Assert.Equal("Note without userId", exception.Message);
        }

        [Fact]
        public async Task CreateNewNote_ThrowsException_WhenNoNotificationMethodsSpecified()
        {
            // Arrange
            var newNoteDto = new NewNoteDto
            {
                Title = "Test Note",
                UserId = 1,
                Content = "Test Content",
                DeletingDate = DateTime.Now.AddHours(3),
                NotificationMethods = new List<NotificationRequestDto>() // No methods specified
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _noteService.CreateNewNote(newNoteDto));
            Assert.Equal("At least one notification method should be specified", exception.Message);
        }

        [Fact]
        public async Task CreateNewNote_ThrowsException_WhenTitleIsEmpty()
        {
            // Arrange
            var newNoteDto = new NewNoteDto
            {
                Title = "", // Invalid title
                UserId = 1,
                Content = "Test Content",
                DeletingDate = DateTime.Now.AddHours(3),
                NotificationMethods = new List<NotificationRequestDto>
                {
                    new NotificationRequestDto
                    {
                        Method = NotificationMethod.Email,
                        Value = "test@example.com"
                    }
                }
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _noteService.CreateNewNote(newNoteDto));
            Assert.Contains("Title is required", exception.Message); // Assuming ValidateNewNote checks for empty title
        }

        [Fact]
        public async Task CreateNewNote_ThrowsException_WhenDeletingDateIsInPast()
        {
            // Arrange
            var newNoteDto = new NewNoteDto
            {
                Title = "Test Note",
                UserId = 1,
                Content = "Test Content",
                DeletingDate = DateTime.UtcNow.AddHours(-1), // Invalid date
                NotificationMethods = new List<NotificationRequestDto>
                {
                    new NotificationRequestDto
                    {
                        Method = NotificationMethod.Email,
                        Value = "test@example.com"
                    }
                }
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _noteService.CreateNewNote(newNoteDto));
            Assert.Contains("Expire date must be in the future", exception.Message); // Assuming ValidateNewNote checks for past date
        }

        [Fact]
        public async Task UpdateNote_UpdatesNote()
        {
            var note = new Note 
            { 
                Id = Guid.NewGuid(), 
                Title = "Test Note",
                UserId = 1,
                Content = "Test Content",
                ExpireDate = DateTime.UtcNow.AddHours(10),
                NotificationMethodTypes = new List<NotificationMethod> { NotificationMethod.Email }
            };

            _mockRepository.Setup(r => r.GetNote(note.Id)).ReturnsAsync(note);
            _mockRepository.Setup(r => r.UpdateNote(note)).Returns(Task.CompletedTask);

            await _noteService.UpdateNote(note);

            _mockRepository.Verify(r => r.UpdateNote(note), Times.Once);
        }

        [Fact]
        public async Task DeleteNote_DeletesNote()
        {
            var noteId = Guid.NewGuid();

            _mockRepository.Setup(r => r.DeleteNote(noteId)).Returns(Task.CompletedTask);

            await _noteService.DeleteNote(noteId);

            _mockRepository.Verify(r => r.DeleteNote(noteId), Times.Once);
        }
    }
}
