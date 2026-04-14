using System.Windows;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Database.Repositories;

namespace CommunityHub.Ui.Views.Citizen
{
    public partial class SelectCityWindow : Window
    {
        private readonly CityRepository _cityRepository = new CityRepository();
        public City? SelectedCity { get; private set; }
        private Country? _selectedCountry;

        public SelectCityWindow(Country? selectedCountry = null)
        {
            InitializeComponent();
            CitiesList.ItemsSource = selectedCountry == null ? _cityRepository.Search(new CommunityHub.Application.Helpers.Search.CitySearchParameters()) : _cityRepository.GetByCountryId(selectedCountry.Id);
            _selectedCountry = selectedCountry;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var q = SearchTextBox.Text?.Trim();
            var results = string.IsNullOrWhiteSpace(q)
                ? (_selectedCountry == null ? _cityRepository.Search(new CommunityHub.Application.Helpers.Search.CitySearchParameters()) : _cityRepository.GetByCountryId(_selectedCountry.Id))
                : _cityRepository.Search(new CommunityHub.Application.Helpers.Search.CitySearchParameters(_selectedCountry, q));
            CitiesList.ItemsSource = results;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            SelectedCity = CitiesList.SelectedItem as City;
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