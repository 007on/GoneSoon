using GoneSoon.UserService.Domain;
using Microsoft.EntityFrameworkCore;

namespace GoneSoon.UserService.Infrastructure
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<NotificationMethodBase> NotificationMethods => Set<NotificationMethodBase>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotificationMethodBase>()
                .HasDiscriminator<NotificationMethod>("MethodType")
                .HasValue<EmailNotificationMethod>(NotificationMethod.Email)
                .HasValue<SmsNotificationMethod>(NotificationMethod.Sms)
                .HasValue<PushNotificationMethod>(NotificationMethod.Push);

            modelBuilder.Entity<User>()
                .HasMany(u => u.NotificationMethods)
                .WithOne()
                .HasForeignKey(n => n.Id);
        }
    }
}
