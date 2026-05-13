using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.Domain.Users.RepositoryInterfaces;
using CommunityHub.Application.MappersDto;
using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.Services.Users
{
    public class UserService
    {
        private readonly IUserRepository _userRepository
            = Injector.CreateInstance<IUserRepository>();
        private readonly IMapper<User, UserDto> _userMapper
            = Injector.CreateInstance<IMapper<User, UserDto>>();

        public UserDto? Get(long id)
        {
            var user = _userRepository.Get(id);
            return user is null ? null : _userMapper.MapToDto(user);
        }
    }
}
