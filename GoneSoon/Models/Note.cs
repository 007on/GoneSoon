using System.ComponentModel.DataAnnotations;

namespace GoneSoon.Models
{
    public class Note
    {
        public Guid Id { get; set; }
        public long UserId { get; set; }
        public string Content { get; set; }
        public DateTime ExpireDate { get; set; }
        public ICollection<NotificationMethod> NotificationMethodTypes { get; set; } = new HashSet<NotificationMethod>();
        public string Title { get; set; }
    }

    public class NoteMetadata
    {
        public string Title { get; set; }
        public long UserId { get; set; }
    }

    public class NewNoteDto
    {
        public long UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DeletingDate { get; set; }
        public List<NotificationRequestDto> NotificationMethods { get; set; } = [];
    }

    public class NotificationRequestDto
    {
        [Required] 
        public NotificationMethod Method { get; set; }  // Email, Sms, Push и т.д.
        
        [Required, MinLength(3)]
        public string Value { get; set; }               // Адрес email, номер телефона, токен и т.д.
    }


    public enum NotificationMethod
    {
        None = 0,
        Email,
        Sms,
        Push
    }

    public class Notification
    {
        public string Id { get; set; }
        public long UserId { get; set; }
        public string Content { get; set; }
    }

    public static class NoteExtensions
    {
        public static void ValidateNewNote(this NewNoteDto note)
        {
            if (string.IsNullOrEmpty(note.Content))
            {
                throw new ArgumentException("Content is required");
            }

            if (string.IsNullOrWhiteSpace(note.Title))
            {
                throw new ArgumentException("Title is required");
            }

            if (note.DeletingDate < DateTime.UtcNow)
            {
                throw new ArgumentException("Expire date must be in the future");
            }
        }

        public static void ValidateNote(this Note note)
        {
            if (string.IsNullOrEmpty(note.Content))
            {
                throw new ArgumentException("Content is required");
            }

            if (string.IsNullOrWhiteSpace(note.Title))
            {
                throw new ArgumentException("Title is required");
            }

            if (note.ExpireDate < DateTime.UtcNow)
            {
                throw new ArgumentException("Expire date must be in the future");
            }
        }

        public static (Note, List<NotificationMethodBase>) Parse(this NewNoteDto newNote)
        {
            var notificationMethods = newNote.NotificationMethods
                .Select<NotificationRequestDto, NotificationMethodBase>(methodDto =>
                    methodDto.Method switch
                    {
                        NotificationMethod.Email => new EmailNotificationMethod { Value = methodDto.Value },
                        NotificationMethod.Sms => new SmsNotificationMethod { Value = methodDto.Value },
                        NotificationMethod.Push => new PushNotificationMethod { Value = methodDto.Value },
                        _ => new UnknownNotificationMethod { Value = methodDto.Value }
                    }
            ).ToList();

            var newNoteModel = new Note
            {
                UserId = newNote.UserId,
                Title = newNote.Title,
                Content = newNote.Content,
                ExpireDate = newNote.DeletingDate,
                NotificationMethodTypes = notificationMethods.Select(x => x.NotificationMethodType).ToHashSet()
            };

            return (newNoteModel, notificationMethods);
        }
    }

    public class User
    {
        public long Id { get; set; }
    }

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
