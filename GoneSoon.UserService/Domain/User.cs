namespace GoneSoon.UserService.Domain
{
    public class User
    {
        public long Id { get; set; }

        public string Email { get; set; }

        public string? PasswordHash { get; set; }

        public string? OAuthProvider { get; set; }

        public string? OAuthSubjectId { get; set; }

        public List<NotificationMethodBase> NotificationMethods { get; set; } = new();
    }
}
