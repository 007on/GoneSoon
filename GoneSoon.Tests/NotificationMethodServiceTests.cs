using GoneSoon.Models;
using GoneSoon.Repositories;
using GoneSoon.Services;
using Moq;

namespace GoneSoon.Tests
{
    public class NotificationMethodServiceTests
    {
        private readonly Mock<INotificationMethodRepository> _mockRepository;
        private readonly NotificationMethodService _service;

        public NotificationMethodServiceTests()
        {
            _mockRepository = new Mock<INotificationMethodRepository>();
            _service = new NotificationMethodService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetNotificationMethods_ReturnsMethods()
        {
            var noteId = Guid.NewGuid();
            var methods = new List<NotificationMethodBase> { new EmailNotificationMethod() };

            _mockRepository.Setup(r => r.GetNotificationMethods(noteId)).ReturnsAsync(methods);

            var result = await _service.GetNotificationMethods(noteId);

            Assert.Equal(methods, result);
        }

        [Fact]
        public async Task SaveNotificationMethods_SavesMethods()
        {
            var methods = new List<NotificationMethodBase> { new EmailNotificationMethod() };

            _mockRepository.Setup(r => r.SaveNotificationMethods(methods)).Returns(Task.CompletedTask);

            await _service.SaveNotificationMethods(methods);

            _mockRepository.Verify(r => r.SaveNotificationMethods(methods), Times.Once);
        }

        [Fact]
        public void ParseNotificationMethods_ParsesMethods()
        {
            var values = new (string, string)[] { ("Email", "test@example.com") };
            var result = _service.ParseNotificationMethods(values);

            Assert.Single(result);
            Assert.IsType<EmailNotificationMethod>(result[0]);
            Assert.Equal("test@example.com", result[0].Value);
        }

        [Fact]
        public void ParseNotificationMethods_WithNoteId_ParsesMethods()
        {
            var noteId = Guid.NewGuid();
            var values = new List<(string, string)> { ("Email", "test@example.com") };
            var result = _service.ParseNotificationMethods(noteId, values);

            Assert.Single(result);
            Assert.IsType<EmailNotificationMethod>(result[0]);
            Assert.Equal("test@example.com", result[0].Value);
            Assert.Equal(noteId, result[0].NoteId);
        }
    }
}
