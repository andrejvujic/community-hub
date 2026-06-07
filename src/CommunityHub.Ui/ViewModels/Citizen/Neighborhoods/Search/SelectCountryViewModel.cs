using CommunityHub.Application.Services.Dtos.Addresses;
using CommunityHub.Application.Helpers.Search;
using CommunityHub.Application.Database.Repositories.Shared.Addresses;
using CommunityHub.Application.MappersDto.Addresses;
using CommunityHub.Ui.Helpers.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace CommunityHub.Ui.ViewModels.Citizen.Neighborhoods.Search
{
    public class SelectCountryViewModel : INotifyPropertyChanged
    {
        private readonly CountryRepository _countryRepository = new();
        private readonly CountryMapper _countryMapper = new();
        private CountryDto? _selectedCountry;
        private string _searchQuery = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? ConfirmRequested;
        public event EventHandler? CancelRequested;

        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand SearchCommand { get; }

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

        public ObservableCollection<CountryDto> Countries { get; } = new();

        public SelectCountryViewModel(CountryDto? initial = null)
        {
            ConfirmCommand = new RelayCommand<object>(_ => ConfirmRequested?.Invoke(this, EventArgs.Empty));
            CancelCommand = new RelayCommand<object>(_ => CancelRequested?.Invoke(this, EventArgs.Empty));
            SearchCommand = new RelayCommand<object>(_ => LoadCountries(SearchQuery));

            LoadCountries();

            if (initial != null)
            {
                SelectedCountry = initial;
            }
            else if (Countries.Count > 0)
            {
                SelectedCountry = Countries[0];
            }
        }

        public void LoadCountries(string? searchQuery = null)
        {
            var selectedCountryId = _selectedCountry?.Id;

            Countries.Clear();
            var results = string.IsNullOrWhiteSpace(searchQuery) ? _countryRepository.GetAll() : _countryRepository.Search(new CountrySearchParameters(searchQuery));

            foreach (var country in results) Countries.Add(_countryMapper.MapToDto(country));

            SelectedCountry = Countries.FirstOrDefault(c => c.Id == selectedCountryId) ?? Countries.FirstOrDefault();
        }

        protected virtual void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
