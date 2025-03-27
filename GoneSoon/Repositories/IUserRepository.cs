using GoneSoon.Models;

namespace GoneSoon.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserById(long userId);
        Task<User> CreateUser(User user);
    }

}
