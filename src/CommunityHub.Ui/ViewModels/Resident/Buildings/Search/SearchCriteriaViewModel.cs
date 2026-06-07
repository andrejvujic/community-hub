using CommunityHub.Application.Helpers.Search;
using CommunityHub.Application.Services.Addresses;
using CommunityHub.Application.Services.Dtos.Addresses;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Buildings.Search;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Search
{
    public class SearchCriteriaViewModel : BaseViewModel
    {
        private readonly Action<BuildingSearchParameters> _setSerachParameters;
        private readonly CityService _cityService = new();
        private readonly CountryService _countryService = new();

        public SearchCriteriaViewModel(
            Action<BuildingSearchParameters> setSearchParameters,
            IMainNavigationConsumer navigationConsumer,
            BuildingSearchParameters? initialSearchParameters = null
        ) : base(navigationConsumer) {
            _setSerachParameters = setSearchParameters;
            ApplyInitialSearchParameters(initialSearchParameters);

            OpenSelectCountryCommand = new(OpenSelectCountryPage);
            OpenSelectCityCommand = new(OpenSelectCityPage);
            ApplyCommand = new(ApplySearchParameters);
        }

        public RelayCommand OpenSelectCountryCommand { get; }
        public RelayCommand OpenSelectCityCommand { get; }
        public RelayCommand ApplyCommand { get; }
        private void ApplyInitialSearchParameters(BuildingSearchParameters? initialSearchParameters)
        {
            if (initialSearchParameters == null) return;
            LoadInitialCity(initialSearchParameters.CityId);
            LoadInitialCountry(initialSearchParameters.CountryId);
            StreetName = initialSearchParameters.StreetName;
            Neighborhood = initialSearchParameters.Neighborhood;
        }
        
        private void LoadInitialCountry(long? initialCountryId)
        {
            if (initialCountryId is null) return;
            Country = _countryService.GetById(initialCountryId.Value);
        }

        private void LoadInitialCity(long? initialCityId)
        {
            if (initialCityId is null) return;
            City = _cityService.GetById(initialCityId.Value);
        }

        private void ApplySearchParameters()
        {
            _setSerachParameters.Invoke(
                new BuildingSearchParameters(
                    countryId: Country?.Id,
                    cityId: City?.Id,
                    streetName: StreetName,
                    neighborhood: Neighborhood
                )
             );
            Pop();
        }

        private void OpenSelectCountryPage()
            => Push(new SelectCountryPage(SetCountry, Country));

        private void OpenSelectCityPage()
        {
            if (!IsCountrySelected) return;
            Push(new SelectCityPage(SetCity, Country!, City));
        }

        private CountryDto? _country;
        public CountryDto? Country
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

        private CityDto? _city;
        public CityDto? City
        {
            get => _city;
            private set
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

        private void SetCountry(CountryDto? country)
        {
            Country = country;
            City = null;
        }

        private void SetCity(CityDto? city) => City = city;

    }
}
