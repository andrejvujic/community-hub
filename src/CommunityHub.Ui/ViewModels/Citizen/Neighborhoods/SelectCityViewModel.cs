using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Helpers.Search;
using CommunityHub.Application.Database.Repositories.Shared.Addresses;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CommunityHub.Ui.ViewModels.Citizen.Neighborhoods
{
    public class SelectCityViewModel : INotifyPropertyChanged
    {
        private readonly CityRepository _cityRepository = new();
        private Country? _selectedCountry;
        private City? _selectedCity;
        private string _searchQuery = string.Empty;
        private bool _isCitiesEnabled = false;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Country? SelectedCountry
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

        public City? SelectedCity
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

        public ObservableCollection<City> Cities { get; } = new();

        public SelectCityViewModel(Country? selectedCountry = null)
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

            var results = string.IsNullOrWhiteSpace(searchQuery)
                ? _cityRepository.GetByCountryId(SelectedCountry.Id)
                : _cityRepository.Search(new CitySearchParameters(SelectedCountry, searchQuery));

            foreach (var city in results)
                Cities.Add(city);
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
