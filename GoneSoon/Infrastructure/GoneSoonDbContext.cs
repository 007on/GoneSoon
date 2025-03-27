using GoneSoon.Models;
using Microsoft.EntityFrameworkCore;

namespace GoneSoon.Infrastructure
{

    
namespace GoneSoon.Data
    {
        public class GoneSoonDbContext : DbContext
        {
            public GoneSoonDbContext(DbContextOptions<GoneSoonDbContext> options) : base(options) { }

            public DbSet<User> Users { get; set; }
            public DbSet<NotificationMethodBase> NotificationMethods { get; set; }

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
    }

}
