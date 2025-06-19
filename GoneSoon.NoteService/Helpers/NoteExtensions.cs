using GoneSoon.InteractionProtocol.NoteService.Data;
using GoneSoon.NoteService.Domain;

namespace GoneSoon.NoteService.Helpers
{
    public static class NoteExtensions
    {
        public static void ValidateNewNote(this NewNoteDto note)
        {
            if (note.UserId == 0)
            {
                throw new ArgumentException("Note without userId");
            }

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
}
