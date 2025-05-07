namespace GoneSoon.UserService.Dto
{
    public class CreateUserRequest
    {
        public string ExternalId { get; set; } = null!;
        public string Provider { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
