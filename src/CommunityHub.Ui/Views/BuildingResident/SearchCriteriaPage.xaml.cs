using CommunityHub.Application.Domain;
using CommunityHub.Application.Helpers.Search;
using CommunityHub.Ui.Helpers.Navigation;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.BuildingResident
{
    public partial class SearchCriteriaPage : MainNavigatorConsumerPage, INotifyPropertyChanged
    {
        private readonly Action<BuildingSearchParameters> _onSearchParametersApplied;
        public Country? Country { get; private set; }
        public City? City { get; private set; }

        public string StreetName { get; set; } = string.Empty;
        public string Neighborhood { get; set; } = string.Empty;

        public SearchCriteriaPage(
            Action<BuildingSearchParameters> onSearchParametersApplied,
            BuildingSearchParameters? initialSearchParameters = null
        )
        {
            InitializeComponent();
            DataContext = this;

            _onSearchParametersApplied = onSearchParametersApplied;
            ApplyInitialSearchParameters(initialSearchParameters);
            UpdateCityFieldOverlayVisibility();
        }

        private void ApplyInitialSearchParameters(BuildingSearchParameters? initialSearchParameters)
        {
            if (initialSearchParameters == null) return;
            City = initialSearchParameters.City;
            Country = initialSearchParameters.Country;
            StreetName = initialSearchParameters.StreetName;
            Neighborhood = initialSearchParameters.Neighborhood;
        } 

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();

        private void SelectCountryButton_Click(object sender, MouseButtonEventArgs e)
            => Push(new SelectCountryPage(OnCountrySelected));

        private void OnCountrySelected(Country country)
        {
            Country = country;
            OnPropertyChanged(nameof(Country));
            OnPropertyChanged(nameof(SelectedCountryText));

            City = null; // Reset city selection when country selection changes.
            OnPropertyChanged(nameof(City));
            OnPropertyChanged(nameof(SelectedCityText));

            UpdateCityFieldOverlayVisibility();
        }

        public string SelectedCountryText { get => Country == null ? "Izaberite..." : Country.Name; }

        private void SelectCityButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (Country == null) return;
            Push(new SelectCityPage(Country, OnCitySelected, initiallySelectedCity: City));
        }

        private void OnCitySelected(City? city)
        {
            City = city;
            OnPropertyChanged(nameof(City));
            OnPropertyChanged(nameof(SelectedCityText));
        }

        public string SelectedCityText { get => City == null ? "Izaberite..." : City.Name; }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            _onSearchParametersApplied.Invoke(
                new BuildingSearchParameters(
                    country: Country,
                    city: City,
                    streetName: StreetName,
                    neighborhood: Neighborhood
                )
            );
            Pop();
        }

        // City selection should only be possible after selecting a country.
        // If no country is selected, an overlay is shown over the city selection field
        // which makes it unclickable.
        private void UpdateCityFieldOverlayVisibility()
            => CityFieldOverlay.Visibility = Country == null ? Visibility.Visible : Visibility.Hidden;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
