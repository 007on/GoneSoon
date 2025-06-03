using GoneSoon.NoteService.Domain;
using GoneSoon.NoteService.NotificationStrategies;
using GoneSoon.NoteService.Repositories;
using MassTransit.Mediator;

namespace GoneSoon.NoteService.Handlers
{
    public class NoteExpiredNotificationHandler(
        INotificationMethodRepository notificationMethodService, 
        INotificationStrategyFactory notificationStrategyFactory, 
        INoteRepository noteRepository) 
            : MediatorRequestHandler<NoteExpiredNotification>
    {
        private readonly INotificationMethodRepository _notificationMethodRepository = notificationMethodService;
        private readonly INotificationStrategyFactory _notificationStrategyFactory = notificationStrategyFactory;
        private readonly INoteRepository _noteRepository = noteRepository;

        protected override async Task Handle(NoteExpiredNotification keyExpiredNotification, CancellationToken cancellationToken)
        {
            var notificationMethods = await _notificationMethodRepository.GetNotificationMethods(keyExpiredNotification.NoteId);
            var noteMetadata = await _noteRepository.GetNoteMetadata(keyExpiredNotification.NoteId);

            foreach (var notificationMethod in notificationMethods)
            {
                var strategy = _notificationStrategyFactory.GetNotificationStrategy(notificationMethod.NotificationMethodType);

                var notification = new Notification
                {
                    Id = keyExpiredNotification.NoteId.ToString(),
                    UserId = noteMetadata.UserId,
                    Content = noteMetadata.Title
                };
                await strategy.Notify(notification);
            }
        }
    }
}
