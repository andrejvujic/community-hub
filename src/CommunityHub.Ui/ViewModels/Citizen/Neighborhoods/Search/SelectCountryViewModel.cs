using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Helpers.Search;
using CommunityHub.Application.Database.Repositories.Shared.Addresses;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CommunityHub.Ui.ViewModels.Citizen.Neighborhoods.Search
{
    public class SelectCountryViewModel : INotifyPropertyChanged
    {
        private readonly CountryRepository _countryRepository = new();
        private Country? _selectedCountry;
        private string _searchQuery = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;

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

        public ObservableCollection<Country> Countries { get; } = new();

        public SelectCountryViewModel(Country? initial = null)
        {
            SelectedCountry = initial;
            LoadCountries();
        }

        public void LoadCountries(string? searchQuery = null)
        {
            Countries.Clear();
            var results = string.IsNullOrWhiteSpace(searchQuery)
                ? _countryRepository.GetAll()
                : _countryRepository.Search(new CountrySearchParameters(searchQuery));

            foreach (var country in results)
                Countries.Add(country);
        }

        public void Search()
        {
            LoadCountries(SearchQuery);
        }

        protected virtual void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
