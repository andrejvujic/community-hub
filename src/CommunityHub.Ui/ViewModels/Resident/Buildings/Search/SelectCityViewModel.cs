using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Helpers.Search;
using CommunityHub.Application.Services.Addresses;
using CommunityHub.Application.Services.Dtos.Addresses;
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
            set
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
            MainNavigationConsumerPage page,
            CityDto? selectedCity = null
        ) : base(page)
        {
            _setCity = setCity;
            SelectedCountry = selectedCountry;
            SelectedCity = selectedCity;
        }

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

        public bool IsCitySelected(CityDto City)
            => SelectedCity == City;

        public void ResetSelectedCity()
        {
            SelectedCity = null;
            _setCity.Invoke(null);
        }

        public void UpdateSelectedCity(CityDto city)
        {
            SelectedCity = city;
            _setCity.Invoke(SelectedCity);
            Pop();
        }

    }
}
