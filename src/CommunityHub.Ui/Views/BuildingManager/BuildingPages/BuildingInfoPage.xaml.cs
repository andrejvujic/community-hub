using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Domain.Users;
using System.Windows;
using System.Windows.Controls;
using CommunityHub.Ui.ViewModels.BuildingManager.BuildingPages;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.BuildingManager
{
    public partial class BuildingInfoPage : Page
    {
        private readonly BuildingInfoPageViewModel _viewModel;
        public BuildingInfoPage(User user, Building building)
        {
            InitializeComponent();
            _viewModel = new BuildingInfoPageViewModel(user, building);
            DataContext = _viewModel;
        }
        private void LeftArrow_Click(object sender, MouseButtonEventArgs e)
        {
            var homeWindow = Window.GetWindow(this) as HomeWindow;

            if (homeWindow != null)
                homeWindow.HomeFrame.Navigate(new MyBuildingsPage(_viewModel.User));
        }
    }
}