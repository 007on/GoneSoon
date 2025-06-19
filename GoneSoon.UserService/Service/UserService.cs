using GoneSoon.InteractionProtocol.UserService.Data;
using GoneSoon.UserService.Infrastructure;
using GoneSoon.UserService.Mapping;

namespace GoneSoon.UserService.Service
{
    public interface IUserService
    {
        Task<UserDto> FindOrCreateAsync(CreateUserRequest request);
        Task<UserDto?> GetByIdAsync(long id);
    }

    public class UserService(UserDbContext context) : IUserService
    {
        private readonly UserDbContext _context = context;

        public async Task<UserDto> FindOrCreateAsync(CreateUserRequest request)
        {
            var user = request.ToEntity();
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user.ToDto();
        }

        public async Task<UserDto?> GetByIdAsync(long id)
        {
            var user = await _context.Users.FindAsync(id);
            return user?.ToDto();
        }
    }
}
