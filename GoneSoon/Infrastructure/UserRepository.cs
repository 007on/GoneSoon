using GoneSoon.Infrastructure.GoneSoon.Data;
using GoneSoon.Models;
using GoneSoon.Repositories;

namespace GoneSoon.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly GoneSoonDbContext _context;

        public UserRepository(GoneSoonDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserById(long userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User> CreateUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
