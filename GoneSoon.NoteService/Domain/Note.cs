namespace GoneSoon.NoteService.Domain
{
    public class NoteMetadata
    {
        public string Title { get; set; }
        public long UserId { get; set; }
    }

    public class Notification
    {
        public string Id { get; set; }
        public long UserId { get; set; }
        public string Content { get; set; }
    }

}
