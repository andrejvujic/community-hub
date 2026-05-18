using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.MappersDto.Users;
using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Ui.ViewModels.BuildingManager
{
    public class HomeWindowViewModel
    {
        private readonly UserMapper _userMapper = new();
        public UserDto UserDto { get; }
        public User User { get; }
        public string Username => UserDto.Name;
        public HomeWindowViewModel(User user)
        {
            UserDto = _userMapper.MapToDto(user);
            User = user;
        }
    }
}