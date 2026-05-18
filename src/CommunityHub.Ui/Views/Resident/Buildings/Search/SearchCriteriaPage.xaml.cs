using CommunityHub.Application.Helpers.Search;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.Search;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Search
{
    public partial class SearchCriteriaPage : MainNavigatorConsumerPage
    {
        private readonly SearchCriteriaViewModel _viewModel;

        public SearchCriteriaPage(
            Action<BuildingSearchParameters> setSearchParameters,
            BuildingSearchParameters? initialSearchParameters = null
        )
        {
            InitializeComponent();
            _viewModel = new SearchCriteriaViewModel(
                setSearchParameters: setSearchParameters,
                initialSearchParameters: initialSearchParameters
            );
            DataContext = _viewModel;
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();

        private void SelectCountryButton_Click(object sender, MouseButtonEventArgs e)
            => Push(
                    new SelectCountryPage(
                        _viewModel.SetCountry,
                        selectedCountry: _viewModel.Country
                    )
                );

        private void SelectCityButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (!_viewModel.IsCountrySelected) return;
            Push(
                new SelectCityPage(
                        _viewModel.SetCity,
                        selectedCountry: _viewModel.Country!,
                        selectedCity: _viewModel.City
                    )
                );
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ApplySearchParameters();
            Pop();
        }
    }
}
