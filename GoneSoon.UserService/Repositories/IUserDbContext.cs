using GoneSoon.UserService.Domain;
using Microsoft.EntityFrameworkCore;

namespace GoneSoon.UserService.Repositories
{
    public interface IUserDbContext
    {
        DbSet<User> Users { get; }
    }
}
