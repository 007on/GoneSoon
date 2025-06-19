namespace GoneSoon.InteractionProtocol.UserService.Data
{
    public class CreateUserRequest
    {
        public string ExternalId { get; set; }
        public string Provider { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
    }
}