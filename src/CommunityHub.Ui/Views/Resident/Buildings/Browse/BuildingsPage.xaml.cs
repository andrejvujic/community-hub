using CommunityHub.Application.Services.Dtos.Buildings;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.Browse;
using CommunityHub.Ui.Views.Resident.Buildings.Search;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Browse
{
    public partial class BuildingsPage : MainNavigationConsumerPage
    {
        private readonly BuildingsViewModel _viewModel;
        public BuildingsPage(long userId)
        {
            InitializeComponent();
            _viewModel = new BuildingsViewModel(userId, this);
            DataContext = _viewModel;

            _viewModel.UpdateMemberships();
        }

        private void CreateAppartmentAccessRequestButton_Click(object sender, RoutedEventArgs e)
            => _viewModel.OpenSearchBuildingsPage();

        private void OpenBuildingOptionsPageButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is not FrameworkElement element) return;

            if (element.DataContext is not BuildingMembershipGroupDto group) return;

            _viewModel.OpenBuildingOptionsPage(group);
        }
    }
}
