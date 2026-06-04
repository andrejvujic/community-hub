using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Services.Dtos.Addresses;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.ViewModels.Resident.Buildings.Search;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.Search
{
    public partial class SelectCityPage : MainNavigationConsumerPage
    {
        private readonly SelectCityViewModel _viewModel;

        public SelectCityPage(
            Action<CityDto?> setCity,
            CountryDto selectedCountry,
            CityDto? selectedCity = null
        )
        {
            InitializeComponent();
            _viewModel = new SelectCityViewModel(
                setCity: setCity,
                selectedCountry: selectedCountry,
                navigationConsumer: this,
                selectedCity: selectedCity
            );
            DataContext = _viewModel;

            _viewModel.UpdateSearchResults();
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();

        private void CitySearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_viewModel.HasSetSearchText)
                _viewModel.UpdateSearchResults();
        }

    }
}
