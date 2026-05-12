using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.MappersDto.Users
{
    public class UserMapper : IMapper<User, UserDto>
    {
        public UserDto MapToDto(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            return new UserDto(
                user.Id,
                user.Username,
                user.Password,
                user.Name,
                user.Surname,
                user.Role
            );
        }
        public User MapToDomain(UserDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new User(
                dto.Id,
                dto.Username,
                dto.Password,
                dto.Name,
                dto.Surname,
                dto.Role
            );
        }
    }
}