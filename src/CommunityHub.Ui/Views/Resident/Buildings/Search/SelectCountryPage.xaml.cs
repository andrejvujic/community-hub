using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Services.Dtos.Addresses;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.Search;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Search
{
    public partial class SelectCountryPage : MainNavigationConsumerPage
    {
        private readonly SelectCountryViewModel _viewModel;

        public SelectCountryPage(Action<CountryDto?> setCountry, CountryDto? selectedCountry = null)
        {
            InitializeComponent();
            _viewModel = new SelectCountryViewModel(
                setCountry: setCountry,
                page: this,
                selectedCountry: selectedCountry
            );
            DataContext = _viewModel;

            _viewModel.UpdateSearchResults();
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();

        private void SearchButton_Click(object sender, MouseButtonEventArgs e) => _viewModel.UpdateSearchResults();

        private void CountrySearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_viewModel.HasSetSearchText)
                _viewModel.UpdateSearchResults();
        }

        private void CountryItem_Click(object sender, MouseButtonEventArgs e)
        {
            var element = sender as Grid;
            if (element == null) return;

            var country = element.DataContext as CountryDto;
            if (country == null) return;

            if (_viewModel.IsCountrySelected(country))
            {
                _viewModel.ResetSelectedCountry();
                return;
            }

            _viewModel.UpdateSelectedCountry(country);
        }
    }
}
