using GoneSoon.NoteService.Domain;

namespace GoneSoon.NoteService.Repositories;

public interface INotificationMethodRepository
{
    Task SaveNotificationMethods(List<NotificationMethodBase> notificationMethods);
    Task<List<NotificationMethodBase>> GetNotificationMethods(Guid noteId);
}
