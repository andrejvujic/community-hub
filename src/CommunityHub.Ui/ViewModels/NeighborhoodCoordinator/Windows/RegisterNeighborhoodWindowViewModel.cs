using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Services.Addresses;
using CommunityHub.Application.Services.Dtos.Addresses;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Users;
using CommunityHub.Application.Services.Images;
using CommunityHub.Application.Services.Neighborhoods;
using CommunityHub.Application.Services.Users;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows;
using Microsoft.Win32;

namespace CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Windows
{
    public class RegisterNeighborhoodWindowViewModel : INotifyPropertyChanged
    {
        private readonly NeighborhoodService _neighborhoodService = new();
        private readonly CountryService _countryService = new();
        private readonly CityService _cityService = new();
        private readonly StreetService _streetService = new();
        private readonly UserService _userService = new();
        private readonly ImageService _imageService = new();
        private readonly StreetSectionService _streetSectionService = new();

        public UserDto? Coordinator { get; private set; }

        public ObservableCollection<CountryDto> Countries { get; } = new();
        public ObservableCollection<CityDto> Cities { get; } = new();
        public ObservableCollection<StreetDto> Streets { get; } = new();
        public ObservableCollection<StreetSectionDto> AddedStreetSections { get; } = new();
        public ObservableCollection<string> SelectedImagePaths { get; } = new();

        public RelayCommand AddStreetSectionCommand { get; }
        public RelayCommand<StreetSectionDto> RemoveStreetSectionCommand { get; }
        public RelayCommand SelectImagesCommand { get; }
        public RelayCommand<string> RemoveImageCommand { get; }
        public RelayCommand RegisterCommand { get; }
        public RelayCommand CancelCommand { get; }

        public RegisterNeighborhoodWindowViewModel(long coordinatorId)
        {
            Coordinator = _userService.Get(coordinatorId);
            LoadCountries();

            AddStreetSectionCommand = new RelayCommand(ExecuteAddStreetSection);
            RemoveStreetSectionCommand = new RelayCommand<StreetSectionDto>(ExecuteRemoveStreetSection);
            SelectImagesCommand = new RelayCommand(ExecuteSelectImages);
            RemoveImageCommand = new RelayCommand<string>(ExecuteRemoveImage);
            RegisterCommand = new RelayCommand(ExecuteRegister);
            CancelCommand = new RelayCommand(ExecuteCancel);
        }

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanRegister));
            }
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set
            {
                if (_description == value) return;
                _description = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanRegister));
            }
        }

        private CountryDto? _selectedCountry;
        public CountryDto? SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                if (_selectedCountry == value) return;
                _selectedCountry = value;
                OnPropertyChanged();
                LoadCities();
                OnPropertyChanged(nameof(IsCityEnabled));
                OnPropertyChanged(nameof(CanRegister));
            }
        }

        private CityDto? _selectedCity;
        public CityDto? SelectedCity
        {
            get => _selectedCity;
            set
            {
                if (_selectedCity == value) return;
                _selectedCity = value;
                OnPropertyChanged();
                LoadStreets();
                OnPropertyChanged(nameof(IsStreetEnabled));
                OnPropertyChanged(nameof(CanRegister));
            }
        }

        private StreetDto? _selectedStreet;
        public StreetDto? SelectedStreet
        {
            get => _selectedStreet;
            set
            {
                if (_selectedStreet == value) return;
                _selectedStreet = value;
                OnPropertyChanged();
            }
        }

        private ParityType _selectedParityType = ParityType.Both;
        public ParityType SelectedParityType
        {
            get => _selectedParityType;
            set
            {
                if (_selectedParityType == value) return;
                _selectedParityType = value;
                OnPropertyChanged();
            }
        }

        private string _startNumberInput = string.Empty;
        public string StartNumberInput
        {
            get => _startNumberInput;
            set
            {
                if (_startNumberInput == value) return;
                _startNumberInput = value;
                OnPropertyChanged();
            }
        }

        private string _endNumberInput = string.Empty;
        public string EndNumberInput
        {
            get => _endNumberInput;
            set
            {
                if (_endNumberInput == value) return;
                _endNumberInput = value;
                OnPropertyChanged();
            }
        }

        public bool IsCityEnabled => SelectedCountry != null && Cities.Count > 0 && !HasStreetSections;
        public bool IsStreetEnabled => SelectedCity != null && Streets.Count > 0;
        public bool IsCountryEnabled => !HasStreetSections;
        public bool HasStreetSections => AddedStreetSections.Count > 0;

        public string LocationLockTooltip => HasStreetSections
            ? "Ne možete menjati lokaciju dok ne obrišete sve dodate ulice."
            : string.Empty;

        public bool CanRegister =>
            !string.IsNullOrWhiteSpace(Name) &&
            !string.IsNullOrWhiteSpace(Description) &&
            SelectedCountry != null &&
            SelectedCity != null &&
            AddedStreetSections.Count > 0 &&
            SelectedImagePaths.Count > 0;

        public void LoadCountries()
        {
            Countries.Clear();
            var countries = _countryService.GetAll();
            foreach (var country in countries)
                Countries.Add(country);
        }

        public void LoadCities()
        {
            Cities.Clear();
            SelectedCity = null;
            Streets.Clear();
            SelectedStreet = null;

            if (SelectedCountry == null) return;

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

            if (SelectedCity == null) return;

            var streets = _streetService.GetByCityId(SelectedCity.Id);
            foreach (var street in streets)
                Streets.Add(street);

            OnPropertyChanged(nameof(IsStreetEnabled));
        }

        private void ExecuteAddStreetSection()
        {
            var error = TryAddStreetSection(StartNumberInput, EndNumberInput, SelectedParityType);
            if (error != null)
            {
                ShowDialog("Greška", error);
                return;
            }

            StartNumberInput = string.Empty;
            EndNumberInput = string.Empty;
        }

        private void ExecuteRemoveStreetSection(StreetSectionDto? section)
        {
            if (section == null) return;
            AddedStreetSections.Remove(section);
            NotifyStreetSectionChanged();
        }

        private void ExecuteSelectImages()
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Images|*.jpg;*.jpeg;*.png",
                Title = "Izaberite slike kvarta"
            };

            if (dialog.ShowDialog() == true)
                AddImages(dialog.FileNames);
        }

        private void ExecuteRemoveImage(string? path)
        {
            if (path == null) return;
            SelectedImagePaths.Remove(path);
            OnPropertyChanged(nameof(CanRegister));
        }

        private void ExecuteRegister()
        {
            var errors = BuildValidationReport();
            if (!string.IsNullOrEmpty(errors))
            {
                ShowDialog("Validacija", "Molimo popunite sledeće nedostatke:\n\n" + errors);
                return;
            }

            var neighborhoodDto = new NeighborhoodDto(
                0,
                Name.Trim(),
                Description.Trim(),
                0,
                SelectedCity!,
                SelectedCountry!,
                Coordinator!
            );

            var createdNeighborhood = _neighborhoodService.Create(neighborhoodDto);
            foreach (var section in AddedStreetSections)
                _streetSectionService.Create(section, createdNeighborhood.Id);
            foreach (var path in SelectedImagePaths)
                _imageService.CreateForNeighborhood(path, createdNeighborhood.Id);

            ShowDialog("Uspeh", $"Kvart '{Name}' je uspešno registrovan!");

            CloseWindow(true);
        }

        private void ExecuteCancel() => CloseWindow(false);

        private string? TryAddStreetSection(string startNumberText, string endNumberText, ParityType side)
        {
            if (SelectedStreet is null)
                return "Izaberite ulicu.";

            if (!int.TryParse(startNumberText, out int startNumber) || !int.TryParse(endNumberText, out int endNumber))
                return "Unesite ispravne brojeve.";

            if (startNumber <= 0 || endNumber <= 0)
                return "Brojevi moraju biti veći od 0.";

            if (startNumber >= endNumber)
                return "Početni broj mora biti manji od krajnjeg.";

            AddedStreetSections.Add(new StreetSectionDto(0, startNumber, endNumber, side, SelectedStreet));
            NotifyStreetSectionChanged();

            return null;
        }

        private void AddImages(IEnumerable<string> paths)
        {
            foreach (var path in paths.Where(p => !SelectedImagePaths.Contains(p)))
                SelectedImagePaths.Add(path);

            OnPropertyChanged(nameof(CanRegister));
        }

        private void NotifyStreetSectionChanged()
        {
            OnPropertyChanged(nameof(HasStreetSections));
            OnPropertyChanged(nameof(IsCountryEnabled));
            OnPropertyChanged(nameof(IsCityEnabled));
            OnPropertyChanged(nameof(LocationLockTooltip));
            OnPropertyChanged(nameof(CanRegister));
        }

        private string BuildValidationReport()
        {
            var sb = new StringBuilder();

            if (string.IsNullOrWhiteSpace(Name))
                sb.AppendLine("- Naziv kvarta nije unet.");

            if (string.IsNullOrWhiteSpace(Description))
                sb.AppendLine("- Opis kvarta nije unet.");

            if (SelectedCountry == null)
                sb.AppendLine("- Država nije izabrana.");

            if (SelectedCity == null)
                sb.AppendLine("- Grad nije izabran.");

            if (AddedStreetSections.Count == 0)
                sb.AppendLine("- Morate dodati bar jednu ulicu.");

            if (SelectedImagePaths.Count == 0)
                sb.AppendLine("- Morate dodati bar jednu sliku.");

            return sb.ToString();
        }

        private void ShowDialog(string title, string message)
        {
            var window = GetActiveWindow();
            new InfoDialog(title, message, window).ShowDialog();
        }

        private void CloseWindow(bool result)
        {
            var window = GetActiveWindow();

            if (window is RegisterNeighborhoodWindow view)
            {
                view.DialogResult = result;
                view.Close();
            }
        }

        private Window GetActiveWindow() =>
            System.Windows.Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)
            ?? System.Windows.Application.Current.MainWindow;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}