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
    public class SelectCityViewModel : INotifyPropertyChanged
    {
        private readonly CityRepository _cityRepository = new();
        private readonly CityMapper _cityMapper = new();
        private CountryDto? _selectedCountry;
        private CityDto? _selectedCity;
        private string _searchQuery = string.Empty;
        private bool _isCitiesEnabled = false;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? ConfirmRequested;
        public event EventHandler? CancelRequested;
        public event EventHandler? CloseRequested;

        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand SearchCommand { get; }

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
            ConfirmCommand = new RelayCommand<object>(_ => RequestConfirm());
            CancelCommand = new RelayCommand<object>(_ => RequestCancel());
            CloseCommand = new RelayCommand<object>(_ => RequestClose());
            SearchCommand = new RelayCommand<object>(_ => Search());

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
                : _cityRepository.Search(new CitySearchParameters(SelectedCountry.Id, searchQuery));

            foreach (var city in results)
                Cities.Add(_cityMapper.MapToDto(city));

            if (SelectedCity == null && Cities.Count > 0)
            {
                SelectedCity = Cities[0];
            }
        }

        public void Search()
        {
            LoadCities(SearchQuery);
        }

        public void RequestConfirm()
        {
            ConfirmRequested?.Invoke(this, EventArgs.Empty);
        }

        public void RequestCancel()
        {
            CancelRequested?.Invoke(this, EventArgs.Empty);
        }

        public void RequestClose()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        private void UpdateCitiesEnabledState()
        {
            IsCitiesEnabled = SelectedCountry != null;
        }

        protected virtual void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
