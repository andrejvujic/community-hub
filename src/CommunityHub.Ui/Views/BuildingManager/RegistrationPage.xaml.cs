using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Win32;
using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
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
        private List<(int Number, int Floor)> _apartmentEntries = new List<(int Number, int Floor)>();

        private readonly CountryRepository _countryRepository = new CountryRepository();
        private readonly CityRepository _cityRepository = new CityRepository();
        private readonly StreetRepository _streetRepository = new StreetRepository();
        private readonly AddressRepository _addressRepository = new AddressRepository();
        private readonly StreetSectionRepository _streetSectionRepository = new StreetSectionRepository();
        private readonly BuildingRepository _buildingRepository = new BuildingRepository();
        private readonly ImageRepository _imageRepository = new ImageRepository();  
        private readonly ApartmentRepository _apartmentRepository = new ApartmentRepository();
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
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            floorsStackPanel.Children.Add(CreateFloor(1));
            UpdateImagesValidation();
            LoadCountries();
        }

        private void LoadCountries()
        {
            var countries = _countryRepository.GetAll();
            Countries.Clear();
            foreach (var country in countries)
                Countries.Add(country);

            countryComboBox.ItemsSource = Countries;
        }
        private void CountryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadCities();
        }
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
            if (streetComboBox.SelectedItem != null)
                streetNumberTextBox.IsEnabled = true;
            else
            {
                streetNumberTextBox.IsEnabled = false;
            }

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
            {
                MessageBox.Show("Unesite broj zgrade.");
                return false;
            }

            if (!int.TryParse(text, out var number))
            {
                MessageBox.Show("Niste uneli validan podatak za broj ulice.");
                return false;
            }

            if (number < StartNumber || number > EndNumber)
            {
                MessageBox.Show($"Raspon ulice je od broja {StartNumber} do {EndNumber}.");
                return false;
            }

            if (OccupiedNumbers != null && OccupiedNumbers.Contains(number))
            {
                MessageBox.Show("Uneli ste postojeći broj zgrade.");
                return false;
            }

            return true;
        }
        private void FloorsUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (floorsStackPanel == null)
                return;

            int newValue = stepperFloorsUpDown.Value ?? 1;
            int currentCount = floorsStackPanel.Children.Count;

            if (newValue > currentCount)
            {
                for (int i = currentCount + 1; i <= newValue; i++)
                {
                    floorsStackPanel.Children.Add(CreateFloor(i));
                }
            }
            else
            {
                while (floorsStackPanel.Children.Count > newValue)
                {
                    floorsStackPanel.Children.RemoveAt(floorsStackPanel.Children.Count - 1);
                }
            }
        }
        private GroupBox CreateFloor(int number)
        {
            var groupBox = new GroupBox
            {
                Header = $"Sprat {number} - stanovi",
                Tag = number,
                Margin = new Thickness(15, 10, 15, 20),
                Height = 140
            };

            var apartmentsPanel = new StackPanel { Orientation = Orientation.Horizontal, Tag = 1, Margin = new Thickness(4,0,4,0) };
            apartmentsPanel.Children.Add(ApartmentTextBox()); // inicijalno dodajemo jedan stan

            var apartmentsScroll = new ScrollViewer
            {
                Content = apartmentsPanel,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                VerticalScrollBarVisibility = ScrollBarVisibility.Disabled,
                CanContentScroll = false,
                Padding = new Thickness(8, 4, 8, 4),
                Margin = new Thickness(10,10,10,0)
            };
            

            var addButton = AddApartmentButton();
            addButton.Click += (s, e) =>
            {
                apartmentsPanel.Children.Add(ApartmentTextBox());
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

            var row = new Grid();
            row.Margin = new Thickness(8);                     
            row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); 
            row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });                      
   
            Grid.SetColumn(apartmentsScroll, 0);
            Grid.SetColumn(buttonPanel, 1);
            row.Children.Add(apartmentsScroll);
            row.Children.Add(buttonPanel);

            groupBox.Content = row;
            return groupBox;
        }
        private  TextBox ApartmentTextBox()
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
        private  Button AddApartmentButton()
        {
            return new Button
            {
                Content = "+",
                Width = 30,
                Height = 30,
                Margin = new Thickness(4, 0, 0, 0)
            };
        }
        private  Button RemoveApartmentButton()
        {
            return new Button
            {
                Content = "-",
                Width = 30,
                Height = 30,
            };
        }
        private bool IsApartmentNumbersUnique()
        {
            foreach(var number in ApartmentNumbers)
            {
                if (ApartmentNumbers.Count(n => n == number) > 1)
                {
                    MessageBox.Show("Brojevi stanova moraju biti jedinstveni.");
                    return false;
                }
            }

            return true;
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
                        else
                        {
                            int num = int.Parse(apartmentTextBox.Text);
                            ApartmentNumbers.Add(num);
                            _apartmentEntries.Add((Number: num, Floor: floorNumber));
                        }
                    }
                }
            }

            if(!IsApartmentNumbersUnique()) return false;
            return true;
        }
        private bool IsApartmentNumberValid(string text)
        {
            if (string.IsNullOrEmpty(text.Trim()))
            {
                MessageBox.Show("Unesite broj stana.");
                return false;
            }
            if (!int.TryParse(text, out var number))
            {
                MessageBox.Show("Niste uneli validan podatak za broj stana.");
                return false;
            }
            if (number < 1)
            {
                MessageBox.Show("Broj stana mora biti pozitivan.");
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
                foreach(string filePath in dialog.FileNames)
                {
                    if (!SelectedImagePaths.Contains(filePath))
                    {
                        SelectedImagePaths.Add(filePath);
                    }
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
            if(countryComboBox.SelectedItem == null)
            {
                MessageBox.Show("Niste odabrali državu.");
                return false;
            }

            if (cityComboBox.SelectedItem == null)
            {
                MessageBox.Show("Niste odabrali grad.");
                return false;
            }

            if(streetComboBox.SelectedItem == null)
            {
                MessageBox.Show("Niste odabrali ulicu.");
                return false;
            }

            if(!IsStreetNumberValid()) return false;

            if(neighborhoodTextBox.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Niste uneli naziv naselja.");
                return false;
            }

            if(!AreApartmentNumbersValid()) return false;

            if(SelectedImagePaths.Count == 0)
            {
                MessageBox.Show("Morate izabrati bar jednu sliku zgrade.");
                return false;
            }
              
            return true;
        }
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Da li ste sigurni da želite da otkažete registraciju? Svi uneseni podaci će biti izgubljeni.", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                NavigationService.GoBack();
            }
        }
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsFormValid()) return;

            if(!MessageBox.Show("Da li ste sigurni da želite da potvrdite registraciju zgrade?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question).Equals(MessageBoxResult.Yes))
            {
                return;
            }

            Country country = (countryComboBox.SelectedItem as Country)!;
            City city = (cityComboBox.SelectedItem as City)!;
            Street street = (streetComboBox.SelectedItem as Street)!;

            Address address = new Address(
                id: 0,
                street: street,
                streetNumber: streetNumberTextBox.Text.Trim(),
                neighborhood: neighborhoodTextBox.Text.Trim(),
                city: city,
                country: country
            );

            address = _addressRepository.Create(address);

            Building building = new Building(
                floorCount: stepperFloorsUpDown.Value ?? 1,
                apartmentCount: ApartmentNumbers.Count,
                manager: _user,
                address: address
            );

            building = _buildingRepository.Create(building);

            foreach (string imagePath in SelectedImagePaths)
            {
                   DomainImage image = new DomainImage(
                   id: 0,
                   path: imagePath,
                   buildingId: building.Id,
                   neighborhoodId: null
                );

                image = _imageRepository.Create(image);
            }

            foreach (var entry in _apartmentEntries)
            {
                Apartment apartment = new Apartment(
                    id: 0,
                    number: entry.Number,
                    floorNumber: entry.Floor,
                    building: building
                );

                apartment = _apartmentRepository.Create(apartment);
            }

            MessageBox.Show("Uspešno ste registrovali zgradu.", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);

            var homeWindow = Window.GetWindow(this) as HomeWindow;
            if (homeWindow != null)
            {
                homeWindow.HomeFrame.Navigate(new MyBuildingsPage(_user));
            }
        }
        

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}