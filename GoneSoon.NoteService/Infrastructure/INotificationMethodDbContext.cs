using GoneSoon.NoteService.Domain;
using Microsoft.EntityFrameworkCore;

namespace GoneSoon.NoteService.Infrastructure
{
    public interface INotificationMethodDbContext
    {
        DbSet<NotificationMethodBase> NotificationMethods { get; set; }

        Task SaveChangesAsync();
    }
}