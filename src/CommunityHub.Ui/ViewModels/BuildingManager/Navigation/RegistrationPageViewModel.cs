using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.Services.Addresses;
using CommunityHub.Application.Services.Dtos.Addresses;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CommunityHub.Ui.ViewModels.BuildingManager.Navigation
{
    public class RegistrationPageViewModel : INotifyPropertyChanged
    {
        private readonly CountryService _countryService;
        private readonly CityService _cityService;
        private readonly StreetService _streetService;
        private readonly AddressService _addressService;
        private readonly StreetSectionService _streetSectionService;
        public User User { get; }
        public ObservableCollection<CountryDto> Countries { get; } = [];
        public ObservableCollection<CityDto> Cities { get; } = [];
        public ObservableCollection<StreetDto> Streets { get; } = [];
        public ObservableCollection<string> SelectedImagePaths { get; } = [];

        public RegistrationPageViewModel(User user)
        {
            User = user;
            _countryService = new CountryService();
            _cityService = new CityService();
            _streetService = new StreetService();
            _addressService = new AddressService();
            _streetSectionService = new StreetSectionService();
        }

        private CountryDto? _selectedCountry;
        public CountryDto? SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                if (_selectedCountry != value)
                {
                    _selectedCountry = value;
                    OnPropertyChanged();
                    LoadCities();
                    OnPropertyChanged(nameof(IsCityEnabled));
                    OnPropertyChanged(nameof(IsCountryValid));
                    OnPropertyChanged(nameof(IsStreetEnabled));
                }
            }
        }

        private CityDto? _selectedCity;
        public CityDto? SelectedCity
        {
            get => _selectedCity;
            set
            {
                if (_selectedCity != value)
                {
                    _selectedCity = value;
                    OnPropertyChanged();
                    LoadStreets();
                    OnPropertyChanged(nameof(IsStreetEnabled));
                    OnPropertyChanged(nameof(IsCityValid));
                    OnPropertyChanged(nameof(CanConfirm));
                }
            }
        }

        private StreetDto? _selectedStreet;
        public StreetDto? SelectedStreet
        {
            get => _selectedStreet;
            set
            {
                if (_selectedStreet != value)
                {
                    _selectedStreet = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsStreetNumberEnabled));

                    StreetNumber = string.Empty;
                    ErrorMessage = null;
                    LoadStreetNumberMetaData();
                    ValidateStreetNumberLive();
                    OnPropertyChanged(nameof(IsStreetValid));
                    OnPropertyChanged(nameof(CanConfirm));
                }
            }
        }

        private string _streetNumber = string.Empty;
        public string StreetNumber
        {
            get => _streetNumber;
            set
            {
                if (_streetNumber != value)
                {
                    _streetNumber = value;
                    ErrorMessage = null;
                    OnPropertyChanged();
                    ValidateStreetNumberLive();
                    OnPropertyChanged(nameof(CanConfirm));
                }
            }
        }

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _imageErrorMessage = string.Empty;
        public string ImageErrorMessage
        {
            get => _imageErrorMessage;
            set
            {
                if (_imageErrorMessage != value)
                {
                    _imageErrorMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _neighborhood = string.Empty;
        public string Neighborhood
        {
            get => _neighborhood;
            set
            {
                if (_neighborhood != value)
                {
                    _neighborhood = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsNeighborhoodValid));
                    OnPropertyChanged(nameof(CanConfirm));

                }
            }
        }

        private bool _areApartmentsValid;
        public bool AreApartmentsValid
        {
            get => _areApartmentsValid;
            set
            {
                if (_areApartmentsValid != value)
                {
                    _areApartmentsValid = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanConfirm));
                }
            }
        }

        public bool IsCityEnabled => SelectedCountry != null && Cities.Count > 0;
        public bool IsStreetEnabled => SelectedCity != null && Streets.Count > 0;
        public bool IsStreetNumberEnabled => SelectedStreet != null;

        public int StartNumber { get; private set; }
        public int EndNumber { get; private set; }
        public List<int> OccupiedNumbers { get; private set; } = [];

        public bool IsCountryValid => SelectedCountry != null;
        public bool IsCityValid => SelectedCity != null;
        public bool IsStreetValid => SelectedStreet != null;
        public bool IsNeighborhoodValid => !string.IsNullOrWhiteSpace(Neighborhood);
        public bool IsImageValid => SelectedImagePaths.Count > 0;

        public bool CanConfirm =>
            IsCountryValid &&
            IsCityValid &&
            IsStreetValid &&
            IsNeighborhoodValid &&
            IsStreetNumberValid() &&
            IsImageValid &&
            AreApartmentsValid;

        public void LoadCountries()
        {
            Countries.Clear();
            SelectedCountry = null;
            Cities.Clear();
            SelectedCity = null;

            var countries = _countryService.GetAll();
            foreach (var country in countries)
                Countries.Add(country);

            OnPropertyChanged(nameof(IsCityEnabled));
        }
        public void LoadCities()
        {
            Cities.Clear();
            SelectedCity = null;
            Streets.Clear();
            SelectedStreet = null;

            if (SelectedCountry == null)
            {
                OnPropertyChanged(nameof(IsCityEnabled));
                OnPropertyChanged(nameof(IsStreetEnabled));
                return;
            }

            var cities = _cityService.GetByCountryId(SelectedCountry.Id);
            foreach (var city in cities)
                Cities.Add(city);

            OnPropertyChanged(nameof(IsCityEnabled));
            OnPropertyChanged(nameof(IsStreetEnabled));
        }
        public void LoadStreets()
        {
            Streets.Clear();
            SelectedStreet = null;

            if (SelectedCity == null)
            {
                OnPropertyChanged(nameof(IsStreetEnabled));
                return;
            }
            var streets = _streetService.GetByCityId(SelectedCity.Id);
            foreach (var street in streets)
                Streets.Add(street);

            OnPropertyChanged(nameof(IsStreetEnabled));
        }
        public void LoadStreetNumberMetaData()
        {
            StartNumber = 0;
            EndNumber = 0;
            OccupiedNumbers = [];

            if (SelectedStreet == null || SelectedCity == null)
                return;

            StartNumber = _streetSectionService.GetStartNumber(SelectedStreet.Id, SelectedCity.Id);
            EndNumber = _streetSectionService.GetEndNumber(SelectedStreet.Id, SelectedCity.Id);
            OccupiedNumbers = _addressService.GetOccupiedStreetNumbers(SelectedStreet.Id, SelectedCity.Id);
        }
        public void ValidateStreetNumberLive()
        {
            ErrorMessage = null;

            if (SelectedStreet == null)
            {
                ErrorMessage = null;
                return;
            }

            var text = StreetNumber?.Trim();

            if (string.IsNullOrEmpty(text))
            {
                ErrorMessage = "Niste uneli broj ulice.";
                return;
            }

            if (!int.TryParse(text, out var number))
            {
                ErrorMessage = "Neispravan format broja ulice.";
                return;
            }

            if (number < StartNumber || number > EndNumber)
            {
                ErrorMessage = $"Raspon ulice je od broja {StartNumber} do {EndNumber}.";
                return;
            }

            if (OccupiedNumbers != null && OccupiedNumbers.Contains(number))
            {
                ErrorMessage = "Uneli ste postojeći broj zgrade.";
                return;
            }
        }
        private bool IsStreetNumberValid()
        {
            if (SelectedStreet == null)
                return false;

            var text = StreetNumber?.Trim();

            if (string.IsNullOrEmpty(text))
                return false;
            if (!int.TryParse(text, out var number))
                return false;
            if (number < StartNumber || number > EndNumber)
                return false;
            if (OccupiedNumbers != null && OccupiedNumbers.Contains(number))
                return false;

            return true;
        }
        public void AddImages(IEnumerable<string> filePaths)
        {
            foreach (var filePath in filePaths)
            {
                if (!SelectedImagePaths.Contains(filePath))
                    SelectedImagePaths.Add(filePath);
            }

            UpdateImageValidation();
        }
        public void RemoveImage(string filePath)
        {
            if (SelectedImagePaths.Contains(filePath))
                SelectedImagePaths.Remove(filePath);

            UpdateImageValidation();
        }
        public void UpdateImageValidation()
        {
            ImageErrorMessage = SelectedImagePaths.Count == 0 ? "Morate dodati bar jednu sliku zgrade." : string.Empty;
            OnPropertyChanged(nameof(IsImageValid));
            OnPropertyChanged(nameof(CanConfirm));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}