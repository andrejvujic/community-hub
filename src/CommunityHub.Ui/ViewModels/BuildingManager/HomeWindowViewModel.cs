using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Ui.ViewModels.BuildingManager
{
    public class HomeWindowViewModel
    {
        public User User { get;}
        public string UserName => User.Name;
        public HomeWindowViewModel(User user)
        {
            User = user;
        }
    }
}
