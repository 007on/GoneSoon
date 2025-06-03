using GoneSoon.InteractionProtocol;

namespace GoneSoon.NoteService.Domain
{
    public abstract class NotificationMethodBase
    {
        public abstract NotificationMethod NotificationMethodType { get; }

        public string Value { get; set; }

        public Guid NoteId { get; set; }

        public long Id { get; set; }

        public static NotificationMethodBase CreateNotificationMethod(string notificationMethod, string value, Guid? noteId = null)
        {
            var noteIdValue = noteId.GetValueOrDefault();

            if (string.IsNullOrWhiteSpace(value))
            {
                return new UnknownNotificationMethod { NoteId = noteIdValue };
            }

            var notificationMethodParsed = Enum.Parse<NotificationMethod>(notificationMethod);

            switch (notificationMethodParsed)
            {
                case NotificationMethod.Email:
                    return new EmailNotificationMethod { NoteId = noteIdValue, Value = value };
                case NotificationMethod.Sms:
                    return new SmsNotificationMethod { NoteId = noteIdValue, Value = value };
                case NotificationMethod.Push:
                    return new PushNotificationMethod { NoteId = noteIdValue, Value = value };
                default:
                    return new UnknownNotificationMethod { NoteId = noteIdValue, Value = value };
            }
        }
    }

    public class UnknownNotificationMethod : NotificationMethodBase
    {
        public override NotificationMethod NotificationMethodType => NotificationMethod.None;
    }

    public class EmailNotificationMethod : NotificationMethodBase
    {
        public override NotificationMethod NotificationMethodType => NotificationMethod.Email;
    }

    public class SmsNotificationMethod : NotificationMethodBase
    {
        public override NotificationMethod NotificationMethodType => NotificationMethod.Sms;
    }

    public class PushNotificationMethod : NotificationMethodBase
    {
        public override NotificationMethod NotificationMethodType => NotificationMethod.Push;
    }
}
