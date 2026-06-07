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
                navigationConsumer: this,
                selectedCountry: selectedCountry
            );
            DataContext = _viewModel;

            _viewModel.UpdateSearchResults();
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();

        private void CountrySearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_viewModel.HasSetSearchText)
                _viewModel.UpdateSearchResults();
        }

    }
}
