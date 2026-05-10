using CommunityHub.Application.Domain;
using CommunityHub.Application.Helpers.Search;
using CommunityHub.Application.Modules.Shared.Core.Services.Addresses;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Search
{
    public class SelectCityViewModel : INotifyPropertyChanged
    {
        private readonly CityService _cityService = new();

        private readonly Action<City?> _setCity;

        public Country SelectedCountry { get; }

        private City? _selectedCity;
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

        public ObservableCollection<City> SearchResults { get; } = new();
        public bool HasSearchResults => SearchResults.Count > 0;

        public SelectCityViewModel(
            Action<City?> setCity,
            Country selectedCountry,
            City? selectedCity = null
        )
        {
            _setCity = setCity;
            SelectedCountry = selectedCountry;
            SelectedCity = selectedCity;
        }

        public string SearchText { get; set; } = string.Empty;
        public bool HasSetSearchText => !string.IsNullOrEmpty(SearchText);

        private CitySearchParameters GetSearchParameters() => new CitySearchParameters(
            country: SelectedCountry,
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

        public bool IsCitySelected(City City)
            => SelectedCity == City;

        public void ResetSelectedCity()
        {
            SelectedCity = null;
            _setCity.Invoke(null);
        }

        public void UpdateSelectedCity(City City)
            => _setCity.Invoke(City);

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
