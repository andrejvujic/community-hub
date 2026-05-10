using CommunityHub.Application.Database.Repositories.Apartments;
using CommunityHub.Application.Database.Repositories.Buildings;
using CommunityHub.Application.Database.Repositories.Shared;
using CommunityHub.Application.Database.Repositories.Shared.Addresses;
using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Domain.Apartments;
using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Domain.Users;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using DomainImage = CommunityHub.Application.Domain.Image;

namespace CommunityHub.Ui.Views.BuildingManager
{
    public partial class RegistrationPage : Page, INotifyPropertyChanged
    {
        private readonly User _user;
        public ObservableCollection<string> SelectedImagePaths { get; set; }
        public ObservableCollection<Country> Countries { get; }
        public ObservableCollection<City> Cities { get; }
        public ObservableCollection<Street> Streets { get; }
        public ObservableCollection<int> ApartmentNumbers { get; }

        private List<(int Number, int Floor)> _apartmentEntries = new();
        private readonly CountryRepository _countryRepository = new();
        private readonly CityRepository _cityRepository = new();
        private readonly StreetRepository _streetRepository = new();
        private readonly AddressRepository _addressRepository = new();
        private readonly StreetSectionRepository _streetSectionRepository = new();
        private readonly BuildingRepository _buildingRepository = new();
        private readonly ImageRepository _imageRepository = new();
        private readonly ApartmentRepository _apartmentRepository = new();

        private string _imageErrorMessage = string.Empty;
        public string ImageErrorMessage
        {
            get => _imageErrorMessage;
            set
            {
                _imageErrorMessage = value;
                OnPropertyChanged(nameof(ImageErrorMessage));
            }
        }
        private int StartNumber { get; set; }
        private int EndNumber { get; set; }
        private List<int>? OccupiedNumbers { get; set; }
        private long? StreetId { get; set; }
        private long? CityId { get; set; }


        public RegistrationPage(User user)
        {
            InitializeComponent();
            _user = user;
            ApartmentNumbers = new ObservableCollection<int>();
            SelectedImagePaths = new ObservableCollection<string>();
            Countries = new ObservableCollection<Country>();
            Cities = new ObservableCollection<City>();
            Streets = new ObservableCollection<Street>();
            DataContext = this;

        }
        private void RegistrationPage_Loaded(object sender, RoutedEventArgs e)
        {
            floorsStackPanel.Children.Add(CreateFloor(1));
            UpdateImagesValidation();
            LoadCountries();
        }
        private void LoadCountries()
        {
            Countries.Clear();
            var countries = _countryRepository.GetAll();

            foreach (var country in countries)
                Countries.Add(country);

            countryComboBox.ItemsSource = Countries;
        }
        private void CountryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => LoadCities();
        private void LoadCities()
        {
            Cities.Clear();

            var selectedCountry = countryComboBox.SelectedItem as Country;
            if (selectedCountry == null)
            {
                cityComboBox.ItemsSource = null;
                cityComboBox.IsEnabled = false;
                return;
            }

            var cities = _cityRepository.GetByCountryId(selectedCountry.Id);
            foreach (var city in cities)
                Cities.Add(city);


            cityComboBox.ItemsSource = Cities;
            cityComboBox.IsEnabled = Cities.Count > 0;
        }
        private void CityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadStreets();
            CityId = (long?)cityComboBox.SelectedValue;
        }
        private void LoadStreets()
        {
            Streets.Clear();

            var selectedCity = cityComboBox.SelectedItem as City;
            if (selectedCity == null)
            {
                streetComboBox.ItemsSource = null;
                streetComboBox.IsEnabled = false;
                return;
            }

            var streets = _streetRepository.GetByCityId(selectedCity.Id);
            foreach (var street in streets)
                Streets.Add(street);

            streetComboBox.ItemsSource = Streets;
            streetComboBox.IsEnabled = Streets.Count > 0;
        }
        private void StreetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            streetNumberTextBox.IsEnabled = streetComboBox.SelectedItem != null;
            streetNumberTextBox.Text = string.Empty;

            StreetId = (long?)streetComboBox.SelectedValue;
            StartNumber = _streetSectionRepository.GetStartNumber(StreetId, CityId);
            EndNumber = _streetSectionRepository.GetEndNumber(StreetId, CityId);
            OccupiedNumbers = _addressRepository.GetOccupiedStreetNumbers(StreetId, CityId);
        }
        private bool IsStreetNumberValid()
        {
            var text = streetNumberTextBox.Text?.Trim();

            if (string.IsNullOrEmpty(text))
                return ShowError("Unesite broj zgrade.");

            if (!int.TryParse(text, out var number))
                return ShowError("Niste uneli validan podatak za broj ulice.");

            if (number < StartNumber || number > EndNumber)
                return ShowError($"Raspon ulice je od broja {StartNumber} do {EndNumber}.");

            if (OccupiedNumbers != null && OccupiedNumbers.Contains(number))
                return ShowError("Uneli ste postojeći broj zgrade.");

            return true;
        }
        private bool ShowError(string message)
        {
            MessageBox.Show(message);
            return false;
        }
        private void FloorsUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (floorsStackPanel == null) return;

            int newValue = floorsUpDown.Value ?? 1;
            int currentCount = floorsStackPanel.Children.Count;

            if (newValue > currentCount)
                AddFloors(currentCount, newValue);
            else
                RemoveFloors(newValue);
        }
        private void AddFloors(int from, int to)
        {
            for (int i = from + 1; i <= to; i++)
                floorsStackPanel.Children.Add(CreateFloor(i));
        }
        private void RemoveFloors(int targetCount)
        {
            while (floorsStackPanel.Children.Count > targetCount)
                floorsStackPanel.Children.RemoveAt(floorsStackPanel.Children.Count - 1);
        }
        private GroupBox CreateFloor(int number)
        {
            var apartmentsPanel = CreateApartmentsPanel();
            var contentRow = CreateFloorContentRow(apartmentsPanel);

            return new GroupBox
            {
                Header = $"Sprat {number} - stanovi",
                Tag = number,
                Margin = new Thickness(15, 10, 15, 20),
                Height = 140,
                Content = contentRow
            };
        }
        private StackPanel CreateApartmentsPanel()
        {
            var panel = new StackPanel { Orientation = Orientation.Horizontal, Tag = 1, Margin = new Thickness(4, 0, 4, 0) };
            panel.Children.Add(CreateApartmentTextBox()); // inicijalno dodajemo jedan stan
            return panel;
        }
        private TextBox CreateApartmentTextBox()
        {
            return new TextBox
            {
                Width = 45,
                Height = 45,
                VerticalContentAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 7, 0),
            };
        }
        private ScrollViewer CreateApartmentsScroll(StackPanel apartmentsPanel)
        {
            return new ScrollViewer
            {
                Content = apartmentsPanel,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                VerticalScrollBarVisibility = ScrollBarVisibility.Disabled,
                CanContentScroll = false,
                Padding = new Thickness(8, 4, 8, 4),
                Margin = new Thickness(10, 10, 10, 0)
            };
        }
        private StackPanel CreateApartmentButtonPanel(StackPanel apartmentsPanel)
        {
            var addButton = AddApartmentButton();
            addButton.Click += (s, e) =>
            {
                apartmentsPanel.Children.Add(CreateApartmentTextBox());
                apartmentsPanel.Tag = apartmentsPanel.Children.Count;
            };

            var removeButton = RemoveApartmentButton();
            removeButton.Click += (s, e) =>
            {
                if (apartmentsPanel.Children.Count > 1)
                {
                    apartmentsPanel.Children.RemoveAt(apartmentsPanel.Children.Count - 1);
                    apartmentsPanel.Tag = apartmentsPanel.Children.Count;
                }
            };

            var buttonPanel = new StackPanel { Orientation = Orientation.Horizontal };
            buttonPanel.Children.Add(removeButton);
            buttonPanel.Children.Add(addButton);
            return buttonPanel;
        }
        private Button AddApartmentButton()
        {
            return new Button
            {
                Content = "+",
                Width = 30,
                Height = 30,
                Margin = new Thickness(4, 0, 0, 0)
            };
        }
        private Button RemoveApartmentButton()
        {
            return new Button
            {
                Content = "-",
                Width = 30,
                Height = 30,
            };
        }
        private Grid CreateFloorContentRow(StackPanel apartmentsPanel)
        {
            var row = new Grid { Margin = new Thickness(8) };
            row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var apartmentsScroll = CreateApartmentsScroll(apartmentsPanel);
            var buttonPanel = CreateApartmentButtonPanel(apartmentsPanel);

            Grid.SetColumn(apartmentsScroll, 0);
            Grid.SetColumn(buttonPanel, 1);
            row.Children.Add(apartmentsScroll);
            row.Children.Add(buttonPanel);
            return row;
        }
        private bool AreApartmentNumbersValid()
        {
            ApartmentNumbers.Clear();
            _apartmentEntries.Clear();
            foreach (GroupBox floorGroup in floorsStackPanel.Children)
            {
                if (floorGroup.Content is Grid grid && grid.Children.OfType<ScrollViewer>().FirstOrDefault()?.Content is StackPanel apartmentsPanel)
                {
                    int floorNumber = (int)floorGroup.Tag;
                    foreach (TextBox apartmentTextBox in apartmentsPanel.Children.OfType<TextBox>())
                    {
                        if (!IsApartmentNumberValid(apartmentTextBox.Text))
                            return false;

                        int num = int.Parse(apartmentTextBox.Text);
                        ApartmentNumbers.Add(num);
                        _apartmentEntries.Add((Number: num, Floor: floorNumber));
                    }
                }
            }

            if (!IsApartmentNumbersUnique()) return false;

            return true;
        }
        private bool IsApartmentNumberValid(string text)
        {
            if (string.IsNullOrEmpty(text.Trim()))
                return ShowError("Unesite broj stana.");

            if (!int.TryParse(text, out var number))
                return ShowError("Niste uneli validan podatak za broj stana.");

            if (number < 1)
                return ShowError("Broj stana mora biti pozitivan.");

            return true;
        }
        private bool IsApartmentNumbersUnique()
        {
            foreach (var number in ApartmentNumbers)
                if (ApartmentNumbers.Count(n => n == number) > 1)
                {
                    MessageBox.Show("Brojevi stanova moraju biti jedinstveni.");
                    return false;
                }

            return true;
        }
        private void ChooseImage_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Image Files|*.jpg;*.jpeg;*.png;",
                Title = "Izaberite slike zgrade"
            };

            if (dialog.ShowDialog() == true)
            {
                foreach (string filePath in dialog.FileNames)
                {
                    if (!SelectedImagePaths.Contains(filePath))
                        SelectedImagePaths.Add(filePath);
                }

                UpdateImagesValidation();
            }
        }
        private void UpdateImagesValidation()
        {
            ImageErrorMessage = SelectedImagePaths.Count == 0 ? "Morate izabrati bar jednu sliku zgrade." : string.Empty;
        }
        private void RemoveImage_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string filePath)
            {
                SelectedImagePaths.Remove(filePath);
                UpdateImagesValidation();
            }
        }
        private bool IsFormValid()
        {
            if (!IsCountrySelected()) return false;
            if (!IsCitySelected()) return false;
            if (!IsStreetSelected()) return false;
            if (!IsStreetNumberValid()) return false;
            if (!IsNeighborhoodEntered()) return false;
            if (!AreApartmentNumbersValid()) return false;
            if (!IsImageSelected()) return false;
            return true;
        }
        private bool IsCountrySelected() =>
            countryComboBox.SelectedItem != null || ShowError("Niste odabrali državu.");
        private bool IsCitySelected() =>
            cityComboBox.SelectedItem != null || ShowError("Niste odabrali grad.");
        private bool IsStreetSelected() =>
            streetComboBox.SelectedItem != null || ShowError("Niste odabrali ulicu.");
        private bool IsNeighborhoodEntered() =>
            !string.IsNullOrWhiteSpace(neighborhoodTextBox.Text) || ShowError("Niste uneli naziv naselja.");
        private bool IsImageSelected() =>
            SelectedImagePaths.Count > 0 || ShowError("Morate izabrati bar jednu sliku zgrade.");
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Da li ste sigurni da želite da otkažete registraciju? Svi uneseni podaci će biti izgubljeni.", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                NavigationService.GoBack();
        }
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsFormValid()) return;

            if (!MessageBox.Show("Da li ste sigurni da želite da potvrdite registraciju zgrade?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question).Equals(MessageBoxResult.Yes))
                return;

            var address = CreateAndSaveAddress();
            var building = CreateAndSaveBuilding(address);
            CreateAndSaveImages(building);
            CreateAndSaveApartments(building);
            
            MessageBox.Show("Uspešno ste registrovali zgradu.", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);

            HomeWindow? homeWindow = Window.GetWindow(this) as HomeWindow;

            if (homeWindow != null)
                homeWindow.HomeFrame.Navigate(new MyBuildingsPage(_user));
        }
        private Address CreateAndSaveAddress()
        {
            var address = new Address(
                id: 0,
                street: (streetComboBox.SelectedItem as Street)!,
                streetNumber: streetNumberTextBox.Text.Trim(),
                neighborhood: neighborhoodTextBox.Text.Trim(),
                city: (cityComboBox.SelectedItem as City)!,
                country: (countryComboBox.SelectedItem as Country)!
            );

            return _addressRepository.Create(address);
        }
        private Building CreateAndSaveBuilding(Address address)
        {
            var building = new Building(
                floorCount: floorsUpDown.Value ?? 1,
                apartmentCount: ApartmentNumbers.Count,
                manager: _user,
                address: address
            );

            return _buildingRepository.Create(building);
        }
        private void CreateAndSaveImages(Building building)
        {
            foreach (string imagePath in SelectedImagePaths)
            {
                DomainImage image = DomainImage.ForBuilding(
                    id: 0,
                    path: imagePath,
                    buildingId: building.Id
                );

                _imageRepository.Create(image);
            }
        }
        private void CreateAndSaveApartments(Building building)
        {
            foreach (var entry in _apartmentEntries)
            {
                Apartment apartment = new Apartment(
                    id: 0,
                    number: entry.Number,
                    floorNumber: entry.Floor,
                    building: building
                );

                _apartmentRepository.Create(apartment);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}