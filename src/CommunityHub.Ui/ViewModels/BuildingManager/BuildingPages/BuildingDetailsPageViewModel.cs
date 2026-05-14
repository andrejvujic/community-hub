using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.Domain.Buildings;

namespace CommunityHub.Ui.ViewModels.BuildingManager.BuildingPages
{
    public class BuildingDetailsPageViewModel
    {
        public User User { get; }
        public Building Building { get; }

        public BuildingDetailsPageViewModel(User user, Building building)
        {
            User = user;
            Building = building;
        }
    }
}