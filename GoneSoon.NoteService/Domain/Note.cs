using System.ComponentModel.DataAnnotations;

namespace GoneSoon.NoteService.Domain
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

    public enum NotificationMethod
    {
        None = 0,
        Email,
        Sms,
        Push
    }

    public class NotificationRequestDto
    {
        [Required]
        public NotificationMethod Method { get; set; }  // Email, Sms, Push и т.д.

        [Required, MinLength(3)]
        public string Value { get; set; }               // Адрес email, номер телефона, токен и т.д.
    }

    public class Notification
    {
        public string Id { get; set; }
        public long UserId { get; set; }
        public string Content { get; set; }
    }

}
