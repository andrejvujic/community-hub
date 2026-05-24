using CommunityHub.Application.Helpers.Search;
using CommunityHub.Application.Services.Addresses;
using CommunityHub.Application.Services.Dtos.Addresses;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Helpers.Navigation;
using System.Collections.ObjectModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Search
{
    public class SelectCountryViewModel : BaseViewModel
    {
        private readonly CountryService _countryService = new();

        private readonly Action<CountryDto?> _setCountry;

        private CountryDto? _selectedCountry;
        public CountryDto? SelectedCountry
        {
            get => _selectedCountry;
            private set
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
            IMainNavigationConsumer navigationConsumer,
            CountryDto? selectedCountry = null
        ) : base(navigationConsumer)
        {
            _setCountry = setCountry;
            SelectedCountry = selectedCountry;

            SearchCommand = new(UpdateSearchResults);
            SelectCountryCommand = new(SelectCountry);
        }

        public RelayCommand SearchCommand { get; }
        public RelayCommand<CountryDto> SelectCountryCommand { get; }

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

        private bool IsCountrySelected(CountryDto country)
            => SelectedCountry == country;

        private void ResetSelectedCountry()
        {
            SelectedCountry = null;
            _setCountry.Invoke(null);
        }

        private void UpdateSelectedCountry(CountryDto country)
        {
            SelectedCountry = country;
            _setCountry.Invoke(SelectedCountry);
        }

        private void SelectCountry(CountryDto? country)
        {
            if (country is null) return;
            if (IsCountrySelected(country))
            {
                ResetSelectedCountry();
                return;
            }

            UpdateSelectedCountry(country);
            Pop();
        }

    }
}
