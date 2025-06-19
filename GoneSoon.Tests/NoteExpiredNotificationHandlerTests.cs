using GoneSoon.InteractionProtocol.NoteService.Data;
using GoneSoon.NoteService.Domain;
using GoneSoon.NoteService.Handlers;
using GoneSoon.NoteService.NotificationStrategies;
using GoneSoon.NoteService.Repositories;
using Moq;

namespace GoneSoon.Tests
{
    public class NoteExpiredNotificationHandlerTests
    {
        private readonly Mock<INotificationMethodRepository> _mockNotificationMethodRepository;
        private readonly Mock<INotificationStrategyFactory> _mockNotificationStrategyFactory;
        private readonly Mock<INoteRepository> _mockNoteRepository;
        private readonly TestableNoteExpiredNotificationHandler _handler;

        public NoteExpiredNotificationHandlerTests()
        {
            _mockNotificationMethodRepository = new Mock<INotificationMethodRepository>();
            _mockNotificationStrategyFactory = new Mock<INotificationStrategyFactory>();
            _mockNoteRepository = new Mock<INoteRepository>();
            _handler = new TestableNoteExpiredNotificationHandler(
                _mockNotificationMethodRepository.Object,
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

            _mockNotificationMethodRepository.Setup(s => s.GetNotificationMethods(noteId)).ReturnsAsync(notificationMethods);
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

            _mockNotificationMethodRepository.Setup(s => s.GetNotificationMethods(noteId)).ReturnsAsync(notificationMethods);
            _mockNoteRepository.Setup(r => r.GetNoteMetadata(noteId)).ReturnsAsync(noteMetadata);

            await _handler.Handle(notification, CancellationToken.None);

            _mockNotificationStrategyFactory.Verify(f => f.GetNotificationStrategy(It.IsAny<NotificationMethod>()), Times.Never);
        }
    }
}
