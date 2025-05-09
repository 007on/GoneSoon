using GoneSoon.NoteService.Domain;

namespace GoneSoon.NoteService.Helpers
{
    public static class NotificationMethodHelper
    {
        public static List<NotificationMethodBase> ParseNotificationMethods((string, string)[] values)
        {
            var result = new List<NotificationMethodBase>();
            foreach (var (notificationMethodName, value) in values)
            {
                result.Add(NotificationMethodBase.CreateNotificationMethod(notificationMethodName, value));
            }

            return result;
        }

        public static List<NotificationMethodBase> ParseNotificationMethods(Guid noteId, List<(string, string)> values)
        {
            var result = new List<NotificationMethodBase>();
            foreach (var (notificationMethodName, value) in values)
            {
                result.Add(NotificationMethodBase.CreateNotificationMethod(notificationMethodName, value, noteId));
            }

            return result;
        }
    }
}
