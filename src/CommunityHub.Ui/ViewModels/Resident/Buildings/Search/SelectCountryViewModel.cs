using CommunityHub.Application.Domain;
using CommunityHub.Application.Helpers.Search;
using CommunityHub.Application.Modules.Shared.Core.Services.Addresses;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Search
{
    public class SelectCountryViewModel : INotifyPropertyChanged
    {
        private readonly CountryService _countryService = new();

        private readonly Action<Country?> _setCountry;

        private Country? _selectedCountry;
        public Country? SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                if (_selectedCountry == value) return;
                _selectedCountry = value;
                OnPropertyChanged(nameof(SelectedCountry));
            }
        }

        public ObservableCollection<Country> SearchResults { get; } = new();
        public bool HasSearchResults => SearchResults.Count > 0;

        public SelectCountryViewModel(
            Action<Country?> setCountry,
            Country? selectedCountry = null
        )
        {
            _setCountry = setCountry;
            SelectedCountry = selectedCountry;
        }

        public string SearchText { get; set; } = string.Empty;
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

        public bool IsCountrySelected(Country country)
            => SelectedCountry == country;

        public void ResetSelectedCountry()
        {
            SelectedCountry = null;
            _setCountry.Invoke(null);
        }

        public void UpdateSelectedCountry(Country country)
            => _setCountry.Invoke(country);

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
