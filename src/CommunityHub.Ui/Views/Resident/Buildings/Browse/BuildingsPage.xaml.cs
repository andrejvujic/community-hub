using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings;
using CommunityHub.Ui.Views.Resident.Buildings.Search;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Browse
{
    public partial class BuildingsPage : MainNavigatorConsumerPage
    {
        private readonly BuildingsViewModel _viewModel;
        public BuildingsPage(long userId)
        {
            InitializeComponent();
            _viewModel = new BuildingsViewModel(userId);
            DataContext = _viewModel;

            _viewModel.UpdateMemberships();
        }

        private void CreateAppartmentAccessRequestButton_Click(object sender, RoutedEventArgs e)
            => Push(new SearchBuildingsPage(_viewModel.UserId));

        private void OpenBuildingOptionsPageButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is not FrameworkElement element) return;

            if (element.DataContext is not BuildingMembershipGroup group) return;

            Push(new BuildingOptionsPage(
                userId: _viewModel.UserId,
                building: group.Building
            ));
        }
    }
}
