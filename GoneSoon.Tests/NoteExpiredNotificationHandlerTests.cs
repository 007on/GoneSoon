using GoneSoon.Handlers;
using GoneSoon.Models;
using GoneSoon.NotificationStrategies;
using GoneSoon.Repositories;
using GoneSoon.Services;
using Moq;

namespace GoneSoon.Tests
{
    public class NoteExpiredNotificationHandlerTests
    {
        private readonly Mock<INotificationMethodService> _mockNotificationMethodService;
        private readonly Mock<INotificationStrategyFactory> _mockNotificationStrategyFactory;
        private readonly Mock<INoteRepository> _mockNoteRepository;
        private readonly NoteExpiredNotificationHandler _handler;

        public NoteExpiredNotificationHandlerTests()
        {
            _mockNotificationMethodService = new Mock<INotificationMethodService>();
            _mockNotificationStrategyFactory = new Mock<INotificationStrategyFactory>();
            _mockNoteRepository = new Mock<INoteRepository>();
            _handler = new NoteExpiredNotificationHandler(
                _mockNotificationMethodService.Object,
                _mockNotificationStrategyFactory.Object,
                _mockNoteRepository.Object);
        }

        [Fact]
        public async Task Handle_SendsNotifications()
        {
            var noteId = Guid.NewGuid();
            var notification = new NoteExpiredNotification(noteId);
            var notificationMethods = new List<NotificationMethodBase> { new EmailNotificationMethod() };
            var noteMetadata = new NoteMetadata { Title = "Test Note", UserId = 1 };

            _mockNotificationMethodService.Setup(s => s.GetNotificationMethods(noteId)).ReturnsAsync(notificationMethods);
            _mockNoteRepository.Setup(r => r.GetNoteMetadata(noteId)).ReturnsAsync(noteMetadata);
            var mockStrategy = new Mock<INotificationStrategy>();
            _mockNotificationStrategyFactory.Setup(f => f.GetNotificationStrategy(NotificationMethod.Email)).Returns(mockStrategy.Object);

            await _handler.Handle(notification, CancellationToken.None);

            mockStrategy.Verify(s => s.Notify(It.IsAny<Notification>()), Times.Once);
        }

        [Fact]
        public async Task Handle_NoNotificationMethods_DoesNotSendNotifications()
        {
            var noteId = Guid.NewGuid();
            var notification = new NoteExpiredNotification(noteId);
            var notificationMethods = new List<NotificationMethodBase>();
            var noteMetadata = new NoteMetadata { Title = "Test Note", UserId = 1 };

            _mockNotificationMethodService.Setup(s => s.GetNotificationMethods(noteId)).ReturnsAsync(notificationMethods);
            _mockNoteRepository.Setup(r => r.GetNoteMetadata(noteId)).ReturnsAsync(noteMetadata);

            await _handler.Handle(notification, CancellationToken.None);

            _mockNotificationStrategyFactory.Verify(f => f.GetNotificationStrategy(It.IsAny<NotificationMethod>()), Times.Never);
        }
    }
}
