using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using Microsoft.Win32;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Pages
{
    /// <summary>
    /// Interaction logic for RegisterNeighborhoodWindow.xaml
    /// </summary>
    public partial class RegisterNeighborhoodWindow : Window
    {     
        private readonly long _coordinatorId;
        private readonly User _user;

        private readonly UserRepository _userRepository = new UserRepository();
        private readonly NeighborhoodRepository _neighborhoodRepository = new NeighborhoodRepository();
        private readonly CountryRepository _countryRepository = new CountryRepository();
        private readonly CityRepository _cityRepository = new CityRepository();
        private readonly StreetRepository _streetRepository = new StreetRepository();
        private readonly StreetSectionRepository _streetSectionRepository = new StreetSectionRepository();
        private readonly ImageRepository _imageRepository = new ImageRepository();

        public ObservableCollection<Country> Countries { get; } = new ObservableCollection<Country>();
        public ObservableCollection<City> Cities { get; } = new ObservableCollection<City>();
        public ObservableCollection<Street> Streets { get; } = new ObservableCollection<Street>();
        public ObservableCollection<StreetSection> AddedStreetSections { get; } = new ObservableCollection<StreetSection>();
        public ObservableCollection<string> SelectedImagePaths { get; } = new ObservableCollection<string>();

        public RegisterNeighborhoodWindow(long coordinatorId)
        {
            InitializeComponent();
            DataContext = this;
            _coordinatorId = coordinatorId;
            User? user = _userRepository.GetById(coordinatorId);
            if (user == null)
            {
                MessageBox.Show("Korisnik nije pronađen.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            }
            _user = user;

            LoadCountries();
        }

        private void LoadCountries()
        {
            Countries.Clear();
            var countries = _countryRepository.GetAll();
            foreach (var country in countries)
            {
                Countries.Add(country);
            }
        }

        private void CountryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadCities();
        }

        private void LoadCities()
        {
            Cities.Clear();
            if (CountryComboBox.SelectedItem is Country selectedCountry)
            {
                var cities = _cityRepository.GetByCountryId(selectedCountry.Id);
                foreach (var city in cities) Cities.Add(city);
            }
            CityComboBox.IsEnabled = Cities.Count > 0;
        }

        private void CityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadStreets();
        }

        private void LoadStreets()
        {
            Streets.Clear();
            if (CityComboBox.SelectedItem is City selectedCity)
            {
                var streets = _streetRepository.GetByCityId(selectedCity.Id);
                foreach (var street in streets) Streets.Add(street);
            }
            StreetComboBox.IsEnabled = Streets.Count > 0;
        }

        private void AddStreetSection_Click(object sender, RoutedEventArgs e)
        {
            if (!(StreetComboBox.SelectedItem is Street street)) return;

            if (!TryParseNumbers(out int start, out int end)) return;

            if (!IsRangeValid(start, end)) return;

            ParityType side = (SideComboBox.SelectedItem as ComboBoxItem)?.Tag as ParityType? ?? ParityType.Both;

            AddedStreetSections.Add(new StreetSection(start, end, side, street));

            StartNumberTextBox.Clear();
            EndNumberTextBox.Clear();
            UpdateComboBoxState();
        }

        private bool TryParseNumbers(out int start, out int end)
        {
            bool isStartValid = int.TryParse(StartNumberTextBox.Text, out start);
            bool isEndValid = int.TryParse(EndNumberTextBox.Text, out end);

            if (isStartValid && isEndValid) return true;

            MessageBox.Show("Unesite ispravne brojeve.");
            return false;
        }

        private bool IsRangeValid(int start, int end)
        {
            if (start < end) return true;

            MessageBox.Show("Početni broj mora biti manji od krajnjeg.");
            return false;
        }

        private void RemoveStreetSection_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var sectionToRemove = button?.Tag as StreetSection;

            if (sectionToRemove != null)
            {
                AddedStreetSections.Remove(sectionToRemove);
            }

            UpdateComboBoxState();
        }

        private void UpdateComboBoxState()
        {
            bool hasStreets = AddedStreetSections.Count > 0;
            CountryComboBox.IsEnabled = !hasStreets;
            CityComboBox.IsEnabled = !hasStreets;
            if (hasStreets)
            {
                CountryComboBox.ToolTip = "Ne možete menjati lokaciju dok ne obrišete sve dodate ulice.";
                CityComboBox.ToolTip = "Ne možete menjati lokaciju dok ne obrišete sve dodate ulice.";
            }
            else
            {
                CountryComboBox.ToolTip = null;
            }
        }

        private void SelectImages_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog 
            { 
                Multiselect = true, 
                Filter = "Images|*.jpg;*.jpeg;*.png",
                Title = "Izaberite slike kvarta"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string path in openFileDialog.FileNames)
                    SelectedImagePaths.Add(path);
            }
        }

        private void RemoveImage_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var pathToRemove = button?.Tag as string;

            if (pathToRemove != null)
            {
                SelectedImagePaths.Remove(pathToRemove);
            }
        }

        private string ValidateInput(string name, string description, City city, Country country)
        {
            StringBuilder validationReport = new StringBuilder();

            if (string.IsNullOrWhiteSpace(name))
                validationReport.AppendLine("- Naziv kvarta nije unet.");

            if (string.IsNullOrWhiteSpace(description))
                validationReport.AppendLine("- Opis kvarta nije unet.");

            if (country == null)
                validationReport.AppendLine("- Država nije izabrana.");

            if (city == null)
                validationReport.AppendLine("- Grad nije izabran.");

            if (AddedStreetSections.Count == 0)
                validationReport.AppendLine("- Morate dodati bar jednu ulicu.");

            if (SelectedImagePaths.Count == 0)
                validationReport.AppendLine("- Morate dodati bar jednu sliku.");

            return validationReport.ToString();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text.Trim();
            string description = DescriptionTextBox.Text.Trim();
            var city = CityComboBox.SelectedItem as City;
            var country = CountryComboBox.SelectedItem as Country;

            string validationErrors = ValidateInput(name, description, city, country);
            if (!string.IsNullOrEmpty(validationErrors))
            {
                MessageBox.Show("Molimo popunite sledeće nedostatke:\n\n" + validationErrors, "Validacija", MessageBoxButton.OK);
                return;
            }
            
            Neighborhood neighborhood = RegisterNeighborhood(name, description, city, country);
            RegisterStreetSections(neighborhood.Id);
            RegisterImages(neighborhood.Id);
            
            MessageBox.Show($"Kvart '{name}' je uspešno registrovan!", "Uspeh");
            this.DialogResult = true;
            this.Close();
        }

        private Neighborhood RegisterNeighborhood(string name, string description, City city, Country country)
        {
            var neighborhood = new Neighborhood(name, description, 0, city, country, _user);
            return _neighborhoodRepository.Create(neighborhood);
        }

        private void RegisterStreetSections(long neighborhoodId)
        {
            foreach (var streetSection in AddedStreetSections)
            {
                _streetSectionRepository.Create(streetSection, neighborhoodId);
            }
        }

        private void RegisterImages(long neighborhoodId)
        {
            foreach (var imagePath in SelectedImagePaths)
            {
                var image = new CommunityHub.Application.Domain.Image(0, imagePath, null, neighborhoodId);
                _imageRepository.Create(image, neighborhoodId, null);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
