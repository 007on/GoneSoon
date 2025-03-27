using GoneSoon.Models;
using GoneSoon.Repositories;

namespace GoneSoon.Services
{
    public class NotificationMethodService : INotificationMethodService
    {
        private readonly INotificationMethodRepository _notificationMethodRepository;

        public NotificationMethodService(INotificationMethodRepository notificationMethodRepository)
        {
            _notificationMethodRepository = notificationMethodRepository;
        }

        public List<NotificationMethodBase> ParseNotificationMethods((string, string)[] values)
        {
            var result = new List<NotificationMethodBase>();
            foreach (var (notificationMethodName, value) in values)
            {
                result.Add(NotificationMethodBase.CreateNotificationMethod(notificationMethodName, value));
            }

            return result;
        }

        public List<NotificationMethodBase> ParseNotificationMethods(Guid noteId, List<(string, string)> values)
        {
            var result = new List<NotificationMethodBase>();
            foreach (var (notificationMethodName, value) in values)
            {
                result.Add(NotificationMethodBase.CreateNotificationMethod(notificationMethodName, value, noteId));
            }

            return result;
        }

        public async Task SaveNotificationMethods(List<NotificationMethodBase> notificationMethods)
        {
            await _notificationMethodRepository.SaveNotificationMethods(notificationMethods);
        }

        public async Task<List<NotificationMethodBase>> GetNotificationMethods(Guid noteId)
        {
            return await _notificationMethodRepository.GetNotificationMethods(noteId);
        }
    }
}
