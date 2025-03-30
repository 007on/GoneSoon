using GoneSoon.Models;
using GoneSoon.NotificationStrategies;
using GoneSoon.Repositories;
using GoneSoon.Services;
using MediatR;

namespace GoneSoon.Handlers
{
    public class NoteExpiredNotificationHandler : INotificationHandler<NoteExpiredNotification>
    {
        private readonly INotificationMethodService _notificationMethodService;
        private readonly INotificationStrategyFactory _notificationStrategyFactory;
        private readonly INoteRepository _noteRepository;

        public NoteExpiredNotificationHandler(INotificationMethodService notificationMethodService, INotificationStrategyFactory notificationStrategyFactory, INoteRepository noteRepository)
        {
            _notificationMethodService = notificationMethodService;
            _notificationStrategyFactory = notificationStrategyFactory;
            _noteRepository = noteRepository;
        }

        public async Task Handle(NoteExpiredNotification keyExpiredNotification, CancellationToken cancellationToken)
        {
            var notificationMethods = await _notificationMethodService.GetNotificationMethods(keyExpiredNotification.NoteId);
            var noteMetadata = await _noteRepository.GetNoteMetadata(keyExpiredNotification.NoteId);

            foreach (var notificationMethod in notificationMethods)
            {
                var strategy = _notificationStrategyFactory.GetNotificationStrategy(notificationMethod.NotificationMethodType);

                Notification notification = new Notification
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
