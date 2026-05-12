using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Ui.ViewModels.BuildingManager.Navigation
{
    public class HomePageViewModel
    {
        public User User { get; }
        public HomePageViewModel(User user)
        {
            User = user;
        }
    }
}