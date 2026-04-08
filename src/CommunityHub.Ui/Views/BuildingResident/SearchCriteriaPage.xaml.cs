using CommunityHub.Application.Domain;
using CommunityHub.Application.Helpers.Search;
using CommunityHub.Ui.Helpers.Navigation;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.BuildingResident
{
    public partial class SearchCriteriaPage : MainNavigatorConsumerPage, INotifyPropertyChanged
    {
        private readonly Action<BuildingSearchParameters> _onSearchParametersChanged;
        public Country? Country { get; private set; }
        public City? City { get; private set; }

        public string StreetText { get; set; } = string.Empty;
        public string NeighborhoodText { get; set; } = string.Empty;

        public SearchCriteriaPage(
            Action<BuildingSearchParameters> onSearchParametersChanged,
            BuildingSearchParameters? initialSearchParameters = null
        )
        {
            InitializeComponent();
            DataContext = this;

            _onSearchParametersChanged = onSearchParametersChanged;
            ApplyInitialSearchParameters(initialSearchParameters);
            UpdateCityFieldOverlayVisibility();
        }

        private void ApplyInitialSearchParameters(BuildingSearchParameters? initialSearchParameters)
        {
            if (initialSearchParameters == null) return;
            City = initialSearchParameters.City;
            Country = initialSearchParameters.Country;
            StreetText = initialSearchParameters.StreetText;
            NeighborhoodText = initialSearchParameters.NeighborhoodText;
        } 

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();

        private void SelectCountryButton_Click(object sender, MouseButtonEventArgs e)
            => Push(new SelectCountryPage(OnCountrySelected));

        private void OnCountrySelected(Country country)
        {
            Country = country;
            OnPropertyChanged(nameof(Country));
            OnPropertyChanged(nameof(SelectedCountryText));

            UpdateCityFieldOverlayVisibility();
        }

        public string SelectedCountryText { get => Country == null ? "Izaberite..." : Country.Name; }

        private void SelectCityButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (Country == null) return;
            Push(new SelectCityPage(Country, OnCitySelected));
        }

        private void OnCitySelected(City city)
        {
            City = city;
            OnPropertyChanged(nameof(City));
            OnPropertyChanged(nameof(SelectedCityText));
        }

        public string SelectedCityText { get => City == null ? "Izaberite..." : City.Name; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void PerformSearchButton_Click(object sender, RoutedEventArgs e)
        {
            _onSearchParametersChanged.Invoke(
                new BuildingSearchParameters(
                    country: Country,
                    city: City,
                    streetText: StreetText,
                    neighborhoodText: NeighborhoodText
                )
            );
            Pop();
        }

        private void UpdateCityFieldOverlayVisibility()
            => CityFieldOverlay.Visibility = Country == null ? Visibility.Visible : Visibility.Hidden;
    }
}
