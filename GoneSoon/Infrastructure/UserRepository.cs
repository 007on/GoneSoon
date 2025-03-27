using GoneSoon.Infrastructure.GoneSoon.Data;
using GoneSoon.Models;
using GoneSoon.Repositories;
using Microsoft.EntityFrameworkCore;

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

    public class NotificationMethodRepository : INotificationMethodRepository
    {
        private readonly GoneSoonDbContext _context;

        public NotificationMethodRepository(GoneSoonDbContext context)
        {
            _context = context;
        }

        public async Task<NotificationMethodBase> GetMethodByIdAsync(long methodId)
        {
            return await _context.NotificationMethods.FindAsync(methodId);
        }

        public async Task AddMethodAsync(NotificationMethodBase method)
        {
            await _context.NotificationMethods.AddAsync(method);
            await _context.SaveChangesAsync();
        }

        public async Task SaveNotificationMethods(List<NotificationMethodBase> notificationMethods)
        {
            await _context.NotificationMethods.AddRangeAsync(notificationMethods);
            await _context.SaveChangesAsync();
        }

        public Task<List<NotificationMethodBase>> GetNotificationMethods(Guid noteId)
        {
            return _context.NotificationMethods
                .Where(x => x.NoteId == noteId)
                .ToListAsync();
        }
    }
}
