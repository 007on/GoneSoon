using GoneSoon.NoteService.Handlers;
using GoneSoon.NoteService.NotificationStrategies;
using GoneSoon.NoteService.Repositories;

namespace GoneSoon.Tests
{
    public class TestableNoteExpiredNotificationHandler : NoteExpiredNotificationHandler
    {
        public TestableNoteExpiredNotificationHandler(
            INotificationMethodRepository notificationMethodRepository,
            INotificationStrategyFactory notificationStrategyFactory,
            INoteRepository noteRepository)
            : base(notificationMethodRepository, notificationStrategyFactory, noteRepository)
        {
        }

        public new async Task Handle(NoteExpiredNotification notification, CancellationToken cancellationToken)
        {
            await base.Handle(notification, cancellationToken);
        }
    }
}
