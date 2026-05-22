using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.MappersDto.Buildings;
using CommunityHub.Application.Services.Buildings;
using CommunityHub.Application.Services.Dtos.Buildings;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using CommunityHub.Ui.Helpers.Commands;
using System.Windows.Input;
using CommunityHub.Ui.Views.BuildingManager;

namespace CommunityHub.Ui.ViewModels.BuildingManager.Navigation
{
    public class MyBuildingsPageViewModel
    {
        private readonly BuildingService _buildingService;
        private readonly BuildingMapper _buildingMapper = new();
        public User User { get; }
        public ObservableCollection<BuildingDto> Buildings { get; } = [];

        public ICommand LoadBuildingsCommand { get; }
        public ICommand DetailsCommand { get; }
        public Action<Page>? Navigate { get; set; }
        public MyBuildingsPageViewModel(User user)
        {
            User = user;
            _buildingService = new BuildingService();

            LoadBuildingsCommand = new RelayCommand<object?>(_ => LoadBuildings());
            DetailsCommand = new RelayCommand<BuildingDto?>(OpenDetails);
        }
        public void LoadBuildings()
        {
            Buildings.Clear();

            var buildings = _buildingService.GetByManagerId(User.Id);

            foreach (var building in buildings)
                Buildings.Add(building);
        }
        private void OpenDetails(BuildingDto? buildingDto)
        {
            if (buildingDto == null) return;

            var building = _buildingMapper.MapToDomain(buildingDto);
            Navigate?.Invoke(new BuildingDetailsPage(User, building));
        }
    }
}