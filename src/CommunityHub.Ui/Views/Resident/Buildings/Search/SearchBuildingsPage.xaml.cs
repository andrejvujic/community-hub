using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.Search;
using CommunityHub.Ui.Views.Resident.Apartments;
using CommunityHub.Ui.Views.Resident.Buildings.Browse;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Search
{

    public partial class SearchBuildingsPage : MainNavigatorConsumerPage
    {
        private readonly SearchBuildingsViewModel _viewModel;
        public SearchBuildingsPage(long userId)
        {
            InitializeComponent();
            _viewModel = new SearchBuildingsViewModel(userId);
            DataContext = _viewModel;
            _viewModel.UpdateSearchResults();
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();

        private void SearchButton_Click(object sender, MouseButtonEventArgs e)
        {
            Push(
                new SearchCriteriaPage(
                    setSearchParameters: _viewModel.SetSearchParameters,
                    initialSearchParameters: _viewModel.SearchParameters
                ),
                onPop: _ => _viewModel.UpdateSearchResults()
            );
        }
        

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

            var building = element.DataContext as Building;
            if (building == null) return;

            Push(new BuildingDetailsPage(building));
        }

        private void OpenRequestApartmentAccessPageButton_Click(object sender, RoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element == null) return;

            var building = element.DataContext as Building;
            if (building == null) return;

            Push(new RequestApartmentAccessPage(_viewModel.UserId, building));
        }
    }
}
