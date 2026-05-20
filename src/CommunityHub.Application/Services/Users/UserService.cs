using CommunityHub.Application.Domain.RepositoryInterfaces.Users;
using CommunityHub.Application.MappersDto.Users;
using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.Services.Users
{
    public class UserService
    {
        private readonly IUserRepository _userRepository
            = Injector.CreateInstance<IUserRepository>();
        private readonly UserMapper _userMapper = new();

        public UserDto? Get(long id)
        {
            var user = _userRepository.Get(id);
            return user is null ? null : _userMapper.MapToDto(user);
        }
    }
}
