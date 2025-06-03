using GoneSoon.UserService.Domain;
using GoneSoon.UserService.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GoneSoon.UserService.Infrastructure
{
    public class UserDbContext : DbContext, IUserDbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>();
        }
    }
}
