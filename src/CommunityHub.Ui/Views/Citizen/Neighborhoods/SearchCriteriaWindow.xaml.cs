using System.Windows;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Helpers.Search;

namespace CommunityHub.Ui.Views.Citizen.Neighborhoods
{
    public partial class SearchCriteriaWindow : Window
    {
        public NeighborhoodSearchParameters? Result { get; private set; }

        private Country? _selectedCountry;
        private City? _selectedCity;
        private const string SelectCountryPrompt = "Izaberite državu";
        private const string SelectCityPrompt = "Izaberite grad";

        public SearchCriteriaWindow(NeighborhoodSearchParameters? initial = null)
        {
            InitializeComponent();
            SelectCityButton.IsEnabled = false;

            if (initial != null)
            {
                NameTextBox.Text = initial.Name;
                AddressTextBox.Text = initial.Address;

                _selectedCountry = initial.Country;
                _selectedCity = initial.City;
            }

            UpdateSelectButtons();
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
            if (_selectedCountry == null)
                _selectedCity = null;

            UpdateSelectButtons();
        }

        private void SelectCityButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new SelectCityWindow(_selectedCountry);
            if (window.ShowDialog() == true)
            {
                _selectedCity = window.SelectedCity;
                UpdateSelectButtons();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            NameTextBox.Text = string.Empty;
            AddressTextBox.Text = string.Empty;
            _selectedCountry = null;
            _selectedCity = null;

            UpdateSelectButtons();

            Result = null;
            DialogResult = true;
        }

        private void UpdateSelectButtons()
        {
            SelectCountryButton.Content = _selectedCountry?.Name ?? SelectCountryPrompt;
            SelectCityButton.Content = _selectedCity?.Name ?? SelectCityPrompt;
            SelectCityButton.IsEnabled = _selectedCountry != null;
        }
    }
}
