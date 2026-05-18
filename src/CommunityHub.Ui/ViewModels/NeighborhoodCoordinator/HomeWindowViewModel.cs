using CommunityHub.Application.Database.Repositories.Shared;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.Services.Users;

namespace CommunityHub.Ui.ViewModels.NeighborhoodCoordinator
{
    public class HomeWindowViewModel
    {
        private readonly UserService _userService = new();

        public string UserDisplayName { get; private set; } = string.Empty;

        public bool LoadUser(long userId)
        {
            var user = _userService.Get(userId);
            if (user is null) return false;

            UserDisplayName = $"{user.Name} {user.Surname} - Koordinator kvarta";
            return true;
        }
    }
}
