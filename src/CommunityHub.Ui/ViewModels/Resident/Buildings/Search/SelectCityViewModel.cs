using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Helpers.Search;
using CommunityHub.Application.Services.Addresses;
using CommunityHub.Application.Services.Dtos.Addresses;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Helpers.Navigation;
using System.Collections.ObjectModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Search
{
    public class SelectCityViewModel : BaseViewModel
    {
        private readonly CityService _cityService = new();

        private readonly Action<CityDto?> _setCity;

        public CountryDto SelectedCountry { get; }

        private CityDto? _selectedCity;
        public CityDto? SelectedCity
        {
            get => _selectedCity;
            private set
            {
                if (_selectedCity == value) return;
                _selectedCity = value;
                OnPropertyChanged(nameof(SelectedCity));
            }
        }

        public ObservableCollection<CityDto> SearchResults { get; } = new();
        public bool HasSearchResults => SearchResults.Count > 0;

        public SelectCityViewModel(
            Action<CityDto?> setCity,
            CountryDto selectedCountry,
            IMainNavigationConsumer navigationConsumer,
            CityDto? selectedCity = null
        ) : base(navigationConsumer)
        {
            _setCity = setCity;
            SelectedCountry = selectedCountry;
            SelectedCity = selectedCity;

            SearchCommand = new(UpdateSearchResults);
            SelectCityCommand = new(SelectCity);
        }

        public RelayCommand SearchCommand { get; }
        public RelayCommand<CityDto> SelectCityCommand { get; }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText == value) return;
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                OnPropertyChanged(nameof(HasSetSearchText));
            }
        }
        public bool HasSetSearchText => !string.IsNullOrEmpty(SearchText);

        private CitySearchParameters GetSearchParameters() => new CitySearchParameters(
            countryId: SelectedCountry.Id,
            name: SearchText
        );

        public void UpdateSearchResults()
        {
            var cities = _cityService.Search(GetSearchParameters());
            SearchResults.Clear();
            foreach (var City in cities)
                SearchResults.Add(City);

            OnPropertyChanged(nameof(HasSearchResults));
        }

        private bool IsCitySelected(CityDto City)
            => SelectedCity == City;

        private void ResetSelectedCity()
        {
            SelectedCity = null;
            _setCity.Invoke(null);
        }

        private void UpdateSelectedCity(CityDto city)
        {
            SelectedCity = city;
            _setCity.Invoke(SelectedCity);
        }

        private void SelectCity(CityDto? city)
        {
            if (city is null) return;
            if (IsCitySelected(city))
            {
                ResetSelectedCity();
                return;
            }

            UpdateSelectedCity(city);
            Pop();
        }

    }
}
