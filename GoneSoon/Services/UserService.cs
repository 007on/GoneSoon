using GoneSoon.Models;
using GoneSoon.Repositories;

namespace GoneSoon.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> CreateUser()
        {
            return await _userRepository.CreateUser(new User());
        }
    }
}
