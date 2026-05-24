using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Services.Dtos.Buildings;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.Search;
using CommunityHub.Ui.Views.Resident.Apartments;
using CommunityHub.Ui.Views.Resident.Buildings.Browse;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Search
{

    public partial class SearchBuildingsPage : MainNavigationConsumerPage
    {
        private readonly SearchBuildingsViewModel _viewModel;
        public SearchBuildingsPage(long userId)
        {
            InitializeComponent();
            _viewModel = new SearchBuildingsViewModel(userId, this);
            DataContext = _viewModel;
            _viewModel.UpdateSearchResults();
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();

        private void SearchButton_Click(object sender, MouseButtonEventArgs e)
            => _viewModel.OpenSearchCriteriaPage();
        

        private void ResetSearchButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ResetSearchParameters();
            _viewModel.UpdateSearchResults();
            BuildingsScrollViewer.ScrollToTop();
        }

        private void OpenBuildingDetailsPageButton_Click(object sender, RoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element == null) return;

            var building = element.DataContext as BuildingDto;
            if (building == null) return;

            Push(new BuildingDetailsPage(building));
        }

        private void OpenRequestApartmentAccessPageButton_Click(object sender, RoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element == null) return;

            var building = element.DataContext as BuildingDto;
            if (building == null) return;

            Push(new RequestApartmentAccessPage(_viewModel.UserId, building));
        }
    }
}
