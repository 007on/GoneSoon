using GoneSoon.InteractionProtocol.UserService.Data;
using System.Threading.Tasks;

namespace GoneSoon.InteractionProtocol.UserService
{
    public interface IUserServiceClient
    {
        Task<UserDto> CreateUserAsync(CreateUserRequest request);
        string GetGoogleLoginUrl();
        Task<UserDto> GetUserByIdAsync(long id);
        Task<UserDto> HandleGoogleResponseAsync();
    }
}
