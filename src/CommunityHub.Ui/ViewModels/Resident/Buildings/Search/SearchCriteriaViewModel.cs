using CommunityHub.Application.Domain;
using CommunityHub.Application.Helpers.Search;
using System.ComponentModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Search
{
    public class SearchCriteriaViewModel : INotifyPropertyChanged
    {
        private readonly Action<BuildingSearchParameters> _setSerachParameters;

        public SearchCriteriaViewModel(
            Action<BuildingSearchParameters> setSearchParameters,
            BuildingSearchParameters? initialSearchParameters = null
        ) {
            _setSerachParameters = setSearchParameters;
            ApplyInitialSearchParameters(initialSearchParameters);
        }
        private void ApplyInitialSearchParameters(BuildingSearchParameters? initialSearchParameters)
        {
            if (initialSearchParameters == null) return;
            City = initialSearchParameters.City;
            Country = initialSearchParameters.Country;
            StreetName = initialSearchParameters.StreetName;
            Neighborhood = initialSearchParameters.Neighborhood;
        }

        public void ApplySearchParameters()
        {
            _setSerachParameters.Invoke(
                new BuildingSearchParameters(
                    country: Country,
                    city: City,
                    streetName: StreetName,
                    neighborhood: Neighborhood
                )
             );
        }

        private Country? _country;
        public Country? Country
        {
            get => _country;
            private set {
                if (_country == value) return;
                _country = value;
                OnPropertyChanged(nameof(Country));
                OnPropertyChanged(nameof(SelectedCountryText));
                OnPropertyChanged(nameof(IsCountrySelected));
            }
        }

        public string SelectedCountryText { get => Country == null ? "Izaberite..." : Country.Name; }
        public bool IsCountrySelected => Country is not null;

        private City? _city;
        public City? City
        {
            get => _city;
            set
            {
                if (_city == value) return; 
                _city = value;
                OnPropertyChanged(nameof(City));
                OnPropertyChanged(nameof(SelectedCityText));
            }
        }
        public string SelectedCityText { get => City == null ? "Izaberite..." : City.Name; }

        public string StreetName { get; set; } = string.Empty;
        public string Neighborhood { get; set; } = string.Empty;

        public void SetCountry(Country? country)
        {
            Country = country;
            City = null;
        }

        public void SetCity(City? city) => City = city;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
