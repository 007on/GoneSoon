using GoneSoon.Models;

namespace GoneSoon.Services
{
    public interface INotificationMethodService
    {
        Task<List<NotificationMethodBase>> GetNotificationMethods(Guid noteId);
        List<NotificationMethodBase> ParseNotificationMethods((string, string)[] values);
        List<NotificationMethodBase> ParseNotificationMethods(Guid noteId, List<(string, string)> values);
        Task SaveNotificationMethods(List<NotificationMethodBase> notificationMethods);
    }
}