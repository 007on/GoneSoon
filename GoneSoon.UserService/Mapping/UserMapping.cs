using GoneSoon.UserService.Domain;
using GoneSoon.UserService.Dto;

namespace GoneSoon.UserService.Mapping
{
    public static class UserMapping
    {
        public static UserDto ToDto(this User user) => new()
        {
            Id = user.Id,
            ExternalId = user.OAuthSubjectId ?? string.Empty,
            Provider = user.OAuthProvider ?? string.Empty,
            DisplayName = user.Email,
            Email = user.Email
        };

        public static User ToEntity(this CreateUserRequest request) => new()
        {
            OAuthSubjectId = request.ExternalId,
            OAuthProvider = request.Provider,
            Email = request.Email
        };
    }

}
