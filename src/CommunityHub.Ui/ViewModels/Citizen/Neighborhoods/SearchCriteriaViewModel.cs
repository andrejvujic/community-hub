using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Helpers.Search;
using System.ComponentModel;

namespace CommunityHub.Ui.ViewModels.Citizen.Neighborhoods
{
    public class SearchCriteriaViewModel : INotifyPropertyChanged
    {
        private string _nameFilter = string.Empty;
        private string _addressFilter = string.Empty;
        private Country? _selectedCountry;
        private City? _selectedCity;
        private string _countryButtonContent = "Izaberite državu";
        private string _cityButtonContent = "Izaberite grad";
        private bool _isCityEnabled = false;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string NameFilter
        {
            get => _nameFilter;
            set
            {
                if (_nameFilter == value) return;
                _nameFilter = value;
                OnPropertyChanged(nameof(NameFilter));
            }
        }

        public string AddressFilter
        {
            get => _addressFilter;
            set
            {
                if (_addressFilter == value) return;
                _addressFilter = value;
                OnPropertyChanged(nameof(AddressFilter));
            }
        }

        public Country? SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                if (_selectedCountry == value) return;
                _selectedCountry = value;
                OnPropertyChanged(nameof(SelectedCountry));

                if (_selectedCountry == null)
                    SelectedCity = null;

                UpdateCountryButtonContent();
                UpdateCityEnabledState();
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
                UpdateCityButtonContent();
            }
        }

        public string CountryButtonContent
        {
            get => _countryButtonContent;
            private set
            {
                if (_countryButtonContent == value) return;
                _countryButtonContent = value;
                OnPropertyChanged(nameof(CountryButtonContent));
            }
        }

        public string CityButtonContent
        {
            get => _cityButtonContent;
            private set
            {
                if (_cityButtonContent == value) return;
                _cityButtonContent = value;
                OnPropertyChanged(nameof(CityButtonContent));
            }
        }

        public bool IsCityEnabled
        {
            get => _isCityEnabled;
            private set
            {
                if (_isCityEnabled == value) return;
                _isCityEnabled = value;
                OnPropertyChanged(nameof(IsCityEnabled));
            }
        }

        public SearchCriteriaViewModel(NeighborhoodSearchParameters? initial = null)
        {
            if (initial != null)
            {
                NameFilter = initial.Name ?? string.Empty;
                AddressFilter = initial.Address ?? string.Empty;
                SelectedCountry = initial.Country;
                SelectedCity = initial.City;
            }

            UpdateCountryButtonContent();
            UpdateCityButtonContent();
            UpdateCityEnabledState();
        }

        public NeighborhoodSearchParameters? BuildSearchParameters()
        {
            string? name = string.IsNullOrWhiteSpace(NameFilter) ? null : NameFilter.Trim();
            string? address = string.IsNullOrWhiteSpace(AddressFilter) ? null : AddressFilter.Trim();

            return new NeighborhoodSearchParameters(SelectedCountry, SelectedCity, name, address);
        }

        public void Clear()
        {
            NameFilter = string.Empty;
            AddressFilter = string.Empty;
            SelectedCountry = null;
            SelectedCity = null;
        }

        private void UpdateCountryButtonContent()
        {
            CountryButtonContent = SelectedCountry?.Name ?? "Izaberite državu";
        }

        private void UpdateCityButtonContent()
        {
            CityButtonContent = SelectedCity?.Name ?? "Izaberite grad";
        }

        private void UpdateCityEnabledState()
        {
            IsCityEnabled = SelectedCountry != null;
        }

        protected virtual void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
