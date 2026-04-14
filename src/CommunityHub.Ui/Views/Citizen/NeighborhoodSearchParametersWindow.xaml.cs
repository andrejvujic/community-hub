using System.Windows;
using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Helpers.Search;

namespace CommunityHub.Ui.Views.Citizen
{
    public partial class NeighborhoodSearchParametersWindow : Window
    {
        public NeighborhoodSearchParameters? Result { get; private set; }

        private readonly CountryRepository _countryRepository = new CountryRepository();
        private readonly CityRepository _cityRepository = new CityRepository();

        private Country? _selectedCountry;
        private City? _selectedCity;
        private const string SelectCountryPrompt = "Izaberite državu";
        private const string SelectCityPrompt = "Izaberite grad";

        public NeighborhoodSearchParametersWindow(NeighborhoodSearchParameters? initial = null)
        {
            InitializeComponent();
            SelectCityButton.IsEnabled = false;
            if (initial == null) return;

            NameTextBox.Text = initial.Name;
            AddressTextBox.Text = initial.Address;

            _selectedCountry = initial.Country;
            _selectedCity = initial.City;

            SelectCountryButton.Content = _selectedCountry?.Name ?? SelectCountryPrompt;
            SelectCityButton.Content = _selectedCity?.Name ?? SelectCityPrompt;
            SelectCityButton.IsEnabled = _selectedCountry != null;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string? name = string.IsNullOrWhiteSpace(NameTextBox.Text) ? null : NameTextBox.Text.Trim();
            string? address = string.IsNullOrWhiteSpace(AddressTextBox.Text) ? null : AddressTextBox.Text.Trim();

            Result = new NeighborhoodSearchParameters(_selectedCountry, _selectedCity, name, address);
            DialogResult = true;
            Close();
        }

        private void SelectCountryButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new SelectCountryWindow(_selectedCountry);
            if (window.ShowDialog() != true) return;

            _selectedCountry = window.SelectedCountry;
            SelectCountryButton.Content = _selectedCountry?.Name ?? SelectCountryPrompt;
            SelectCityButton.IsEnabled = _selectedCountry != null;
            if (_selectedCountry == null)
            {
                _selectedCity = null;
                SelectCityButton.Content = SelectCityPrompt;
            }
        }

        private void SelectCityButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new SelectCityWindow(_selectedCountry);
            if (window.ShowDialog() == true)
            {
                _selectedCity = window.SelectedCity;
                SelectCityButton.Content = _selectedCity?.Name ?? SelectCityPrompt;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            NameTextBox.Text = string.Empty;
            AddressTextBox.Text = string.Empty;
            SelectCountryButton.Content = SelectCountryPrompt;
            SelectCityButton.Content = SelectCityPrompt;
            SelectCityButton.IsEnabled = false;

            _selectedCountry = null;
            _selectedCity = null;

            Result = null;
            DialogResult = true;
            Close();
        }
    }
}