using CommunityHub.Application.Modules.Shared.API.Public.Users;
using CommunityHub.Application.Modules.Shared.Core.Domain.Users;
using CommunityHub.Application.Modules.Shared.Core.Domain.Users.RepositoryInterfaces;

namespace CommunityHub.Application.Modules.Shared.Core.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User? Get(long id) => _userRepository.Get(id);
    }
}
