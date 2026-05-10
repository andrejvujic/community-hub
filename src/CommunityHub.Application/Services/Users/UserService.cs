using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.Domain.Users.RepositoryInterfaces;

namespace CommunityHub.Application.Services.Users
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User? Get(long id) => _userRepository.Get(id);
    }
}
