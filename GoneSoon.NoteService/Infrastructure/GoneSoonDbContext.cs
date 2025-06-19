using GoneSoon.InteractionProtocol.NoteService.Data;
using GoneSoon.NoteService.Domain;
using Microsoft.EntityFrameworkCore;

namespace GoneSoon.NoteService.Infrastructure;

public class NotificationMethodDbContext(DbContextOptions<NotificationMethodDbContext> options) : DbContext(options), INotificationMethodDbContext
{
    public DbSet<NotificationMethodBase> NotificationMethods { get; set; }

    public async Task SaveChangesAsync()
    {
        await base.SaveChangesAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<NotificationMethodBase>()
            .HasDiscriminator<int>("NotificationType")
            .HasValue<EmailNotificationMethod>((int)NotificationMethod.Email)
            .HasValue<SmsNotificationMethod>((int)NotificationMethod.Sms)
            .HasValue<PushNotificationMethod>((int)NotificationMethod.Push);
    }
}