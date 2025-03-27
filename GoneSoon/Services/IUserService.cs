using GoneSoon.Models;

namespace GoneSoon.Services
{
    public interface IUserService
    {
        Task<User> CreateUser();
    }
}