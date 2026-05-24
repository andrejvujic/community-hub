using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Ui.Navigation;
using CommunityHub.Ui.ViewModels.BuildingManager.BuildingPages;
using System.Windows;
using System.Windows.Controls;

namespace CommunityHub.Ui.Views.BuildingManager
{
    public partial class BuildingDetailsPage : Page
    {
        private readonly BuildingDetailsPageViewModel _viewModel;

        public BuildingDetailsPage(
            User user,
            Building building,
            IAppNavigationService navigationService,
            string? sectionTag = null)
        {
            InitializeComponent();
            _viewModel = new BuildingDetailsPageViewModel(user, building, navigationService, sectionTag);
            DataContext = _viewModel;
            Unloaded += BuildingDetailsPage_Unloaded;
        }

        private void BuildingDetailsPage_Unloaded(object sender, RoutedEventArgs e)
        {
            _viewModel.Dispose();
            Unloaded -= BuildingDetailsPage_Unloaded;
        }
    }
}