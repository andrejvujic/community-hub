using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Domain.Users;
using System.Windows;
using System.Windows.Controls;
using CommunityHub.Ui.ViewModels.BuildingManager.BuildingPages;

namespace CommunityHub.Ui.Views.BuildingManager
{
    public partial class ApartmentAccessRequestsPage : Page
    {
        private readonly ApartmentAccessRequestPageViewModel _viewModel;
        public ApartmentAccessRequestsPage(User user, Building building)
        {
            InitializeComponent();
            _viewModel = new ApartmentAccessRequestPageViewModel(user, building);
            DataContext = _viewModel;
        }
        private void LeftArrow_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var homeWindow = Window.GetWindow(this) as HomeWindow;

            if (homeWindow != null)
                homeWindow.HomeFrame.Navigate(new MyBuildingsPage(_viewModel.User));
        }
    }
}