using GoneSoon.Models;

namespace GoneSoon.Repositories
{
    public interface INotificationMethodRepository
    {
        Task SaveNotificationMethods(List<NotificationMethodBase> notificationMethods);
        Task<List<NotificationMethodBase>> GetNotificationMethods(Guid noteId);
    }

}
