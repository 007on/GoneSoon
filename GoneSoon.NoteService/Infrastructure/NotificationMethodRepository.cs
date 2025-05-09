using GoneSoon.NoteService.Domain;
using GoneSoon.NoteService.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GoneSoon.NoteService.Infrastructure
{
    public class NotificationMethodRepository(INotificationMethodDbContext context) : INotificationMethodRepository
    {
        private readonly INotificationMethodDbContext _context = context;

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
