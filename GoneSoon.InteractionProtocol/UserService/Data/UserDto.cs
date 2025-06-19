namespace GoneSoon.InteractionProtocol.UserService.Data
{
    public class UserDto
    {
        public long Id { get; set; }
        public string ExternalId { get; set; }
        public string Provider { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
    }
}