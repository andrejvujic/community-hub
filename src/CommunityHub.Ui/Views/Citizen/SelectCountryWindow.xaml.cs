using System.Windows;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Database.Repositories;

namespace CommunityHub.Ui.Views.Citizen
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
            var results = string.IsNullOrWhiteSpace(q) ? _countryRepository.GetAll() : _countryRepository.Search(new CommunityHub.Application.Helpers.Search.CountrySearchParameters(q));
            CountriesList.ItemsSource = results;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            SelectedCountry = CountriesList.SelectedItem as Country;
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}