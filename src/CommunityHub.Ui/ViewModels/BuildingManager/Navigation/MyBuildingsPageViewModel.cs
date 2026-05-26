using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.MappersDto.Buildings;
using CommunityHub.Application.Services.Buildings;
using CommunityHub.Application.Services.Dtos.Buildings;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Navigation;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CommunityHub.Ui.ViewModels.BuildingManager.Navigation
{
    public class MyBuildingsPageViewModel
    {
        private readonly BuildingService _buildingService;
        private readonly BuildingMapper _buildingMapper = new();
        private readonly IAppNavigationService _navigationService;
        public User User { get; }
        public ObservableCollection<BuildingDto> Buildings { get; } = [];

        public ICommand LoadBuildingsCommand { get; }
        public ICommand DetailsCommand { get; }
        public MyBuildingsPageViewModel(User user, IAppNavigationService navigationService)
        {
            User = user;
            _navigationService = navigationService;
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

            var building = _buildingMapper.MapToEntity(buildingDto);

            _navigationService.Navigate(new AppNavigationState
            {
                PageTag = "BuildingDetails",
                Building = building,
                SectionTag = "Info"
            });
        }
    }
}