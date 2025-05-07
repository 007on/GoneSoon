using GoneSoon.UserService.Dto;
using GoneSoon.UserService.Infrastructure;
using GoneSoon.UserService.Mapping;

namespace GoneSoon.UserService.Service
{
    public interface IUserService
    {
        Task<UserDto> FindOrCreateAsync(CreateUserRequest request);
        Task<UserDto?> GetByIdAsync(long id);
        //Task<List<UserDto>> GetAllAsync();
    }

    public class UserService : IUserService
    {
        private readonly UserDbContext _context;

        public UserService(UserDbContext context)
        {
            _context = context;
        }

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

        //public async Task<List<UserDto>> GetAllAsync()
        //{
        //    var users = await _context.Users.ToListAsync();
        //    return users.Select(u => u.ToDto()).ToList();
        //}
    }
}
