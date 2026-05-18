using CommunityHub.Application.Services.Dtos.Addresses;
using CommunityHub.Application.Helpers.Search;
using CommunityHub.Application.Database.Repositories.Shared.Addresses;
using CommunityHub.Application.MappersDto.Addresses;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CommunityHub.Ui.ViewModels.Citizen.Neighborhoods.Search
{
    public class SelectCityViewModel : INotifyPropertyChanged
    {
        private readonly CityRepository _cityRepository = new();
        private readonly CountryMapper _countryMapper = new();
        private readonly CityMapper _cityMapper = new();
        private CountryDto? _selectedCountry;
        private CityDto? _selectedCity;
        private string _searchQuery = string.Empty;
        private bool _isCitiesEnabled = false;

        public event PropertyChangedEventHandler? PropertyChanged;

        public CountryDto? SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                if (_selectedCountry == value) return;
                _selectedCountry = value;
                OnPropertyChanged(nameof(SelectedCountry));
                LoadCities();
                UpdateCitiesEnabledState();
            }
        }

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

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                if (_searchQuery == value) return;
                _searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));
            }
        }

        public bool IsCitiesEnabled
        {
            get => _isCitiesEnabled;
            private set
            {
                if (_isCitiesEnabled == value) return;
                _isCitiesEnabled = value;
                OnPropertyChanged(nameof(IsCitiesEnabled));
            }
        }

        public ObservableCollection<CityDto> Cities { get; } = new();

        public SelectCityViewModel(CountryDto? selectedCountry = null)
        {
            if (selectedCountry != null)
            {
                SelectedCountry = selectedCountry;
            }
        }

        public void LoadCities(string? searchQuery = null)
        {
            Cities.Clear();

            if (SelectedCountry == null)
                return;

            var countryDomain = _countryMapper.MapToDomain(SelectedCountry);
            var results = string.IsNullOrWhiteSpace(searchQuery)
                ? _cityRepository.GetByCountryId(SelectedCountry.Id)
                : _cityRepository.Search(new CitySearchParameters(countryDomain, searchQuery));

            foreach (var city in results)
                Cities.Add(_cityMapper.MapToDto(city));
        }

        public void Search()
        {
            LoadCities(SearchQuery);
        }

        private void UpdateCitiesEnabledState()
        {
            IsCitiesEnabled = SelectedCountry != null;
        }

        protected virtual void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
