using System.Windows;
using CommunityHub.Application.Helpers.Search;
using CommunityHub.Application.Database.Repositories.Shared.Addresses;
using CommunityHub.Application.Domain.Addresses;

namespace CommunityHub.Ui.Views.Citizen.Neighborhoods
{
    public partial class SelectCountryWindow : Window
    {
        private readonly CountryRepository _countryRepository = new CountryRepository();
        public Country? SelectedCountry { get; private set; }

        public SelectCountryWindow(Country? initial = null)
        {
            InitializeComponent();
            CountriesList.ItemsSource = _countryRepository.GetAll();
            SelectedCountry = initial;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var q = SearchTextBox.Text?.Trim();
            var results = string.IsNullOrWhiteSpace(q)
                ? _countryRepository.GetAll()
                : _countryRepository.Search(new CountrySearchParameters(q));
            CountriesList.ItemsSource = results;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            SelectedCountry = CountriesList.SelectedItem as Country;
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}