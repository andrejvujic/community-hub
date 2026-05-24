using CommunityHub.Application.Helpers.Search;
using CommunityHub.Application.Services.Addresses;
using CommunityHub.Application.Services.Dtos.Addresses;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Search
{
    public class SelectCountryViewModel : INotifyPropertyChanged
    {
        private readonly CountryService _countryService = new();

        private readonly Action<CountryDto?> _setCountry;

        private CountryDto? _selectedCountry;
        public CountryDto? SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                if (_selectedCountry == value) return;
                _selectedCountry = value;
                OnPropertyChanged(nameof(SelectedCountry));
            }
        }

        public ObservableCollection<CountryDto> SearchResults { get; } = new();
        public bool HasSearchResults => SearchResults.Count > 0;

        public SelectCountryViewModel(
            Action<CountryDto?> setCountry,
            CountryDto? selectedCountry = null
        )
        {
            _setCountry = setCountry;
            SelectedCountry = selectedCountry;
        }

        private string _searchText = string.Empty;
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

        private CountrySearchParameters GetSearchParameters() => new CountrySearchParameters(name: SearchText);

        public void UpdateSearchResults()
        {
            var countries = _countryService.Search(GetSearchParameters());
            SearchResults.Clear();
            foreach (var country in countries)
                SearchResults.Add(country);

            OnPropertyChanged(nameof(HasSearchResults));
        }

        public bool IsCountrySelected(CountryDto country)
            => SelectedCountry == country;

        public void ResetSelectedCountry()
        {
            SelectedCountry = null;
            _setCountry.Invoke(null);
        }

        public void UpdateSelectedCountry(CountryDto country)
        {
            SelectedCountry = country;
            _setCountry.Invoke(SelectedCountry);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
