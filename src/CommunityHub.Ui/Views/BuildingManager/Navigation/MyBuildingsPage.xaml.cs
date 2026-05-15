using CommunityHub.Application.MappersDto.Buildings;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.Services.Dtos.Buildings;
using CommunityHub.Ui.ViewModels.BuildingManager.Navigation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.BuildingManager
{
    public partial class MyBuildingsPage : Page
    {
        private readonly MyBuildingsPageViewModel _viewModel;
        private readonly BuildingMapper _buildingMapper = new();
        public MyBuildingsPage(User user)
        {
            InitializeComponent();
            _viewModel = new MyBuildingsPageViewModel(user);
            DataContext = _viewModel;
        }
        private void MyBuildingsPage_Loaded(object sender, RoutedEventArgs e) => _viewModel.LoadBuildings();
        private void Details_Click(object sender, MouseButtonEventArgs e)
        {
            var tb = sender as TextBlock;
            if (tb == null) return;

            var buildingDto = tb.DataContext as BuildingDto;
            if (buildingDto == null) return;

            var window = Window.GetWindow(this) as HomeWindow;

            if (window != null)
                window.HomeFrame.Navigate(new BuildingDetailsPage(_viewModel.User, _buildingMapper.MapToDomain(buildingDto)));
        }
    }
}