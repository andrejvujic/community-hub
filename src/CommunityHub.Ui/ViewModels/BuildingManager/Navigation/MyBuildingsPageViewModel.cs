using CommunityHub.Application;
using CommunityHub.Application.MappersDto.Buildings;
using CommunityHub.Application.Domain.Buildings.RepositoryInterfaces;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.Services.Buildings;
using CommunityHub.Application.Services.Dtos.Buildings;
using System.Collections.ObjectModel;

namespace CommunityHub.Ui.ViewModels.BuildingManager.Navigation
{
    public class MyBuildingsPageViewModel
    {
        private readonly BuildingService _buildingService;
        private readonly BuildingMapper _buildingMapper = new();
        public User User { get; }
        public ObservableCollection<BuildingDto> Buildings { get; } = new();
        public MyBuildingsPageViewModel(User user)
        {
            User = user;
            _buildingService = new BuildingService(Injector.CreateInstance<IBuildingRepository>());
        }
        public void LoadBuildings()
        {
            Buildings.Clear();

            var buildings = _buildingService.GetByManagerId(User.Id);

            foreach (var building in buildings)
                Buildings.Add(_buildingMapper.MapToDto(building));
        }
    }
}
