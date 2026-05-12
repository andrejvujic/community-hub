
using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Ui.ViewModels.BuildingManager.Navigation
{
    public class AssemblyPageViewModel
    {
        public User User { get; }
        public AssemblyPageViewModel(User user)
        {
            User = user;
        }
    }
}
