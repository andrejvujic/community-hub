using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ObservableCollection<Country> Countries { get; }
        public ObservableCollection<City> Cities { get; }
        public ObservableCollection<Street> Streets { get; }

        public ObservableCollection<StreetSection> AddedStreetSections { get; }
        public ObservableCollection<string> SelectedImagePaths { get; set; }

        public RegisterNeighborhoodWindow(long coordinatorId)
        {
            InitializeComponent();
            _coordinatorId = coordinatorId;
            User? user = _userRepository.GetById(coordinatorId);
            if (user == null)
            {
                MessageBox.Show("Korisnik nije pronađen.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            }
            _user = user;

            Countries = new ObservableCollection<Country>();
            Cities = new ObservableCollection<City>();
            Streets = new ObservableCollection<Street>();

            AddedStreetSections = new ObservableCollection<StreetSection>();
            SelectedImagePaths = new ObservableCollection<string>();

            StreetSectionsListBox.ItemsSource = AddedStreetSections;
            ImagesListBox.ItemsSource = SelectedImagePaths;
            LoadCountries();
        }

        private void LoadCountries()
        {
            var countries = _countryRepository.GetAll();
            Countries.Clear();

            foreach (var country in countries)
            {
                Countries.Add(country);
            }

            CountryComboBox.ItemsSource = Countries;
        }

        private void CountryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadCities();
        }

        private void LoadCities()
        {
            Cities.Clear();

            var selectedCountry = CountryComboBox.SelectedItem as Country;
            if (selectedCountry == null)
            {
                CityComboBox.ItemsSource = null;
                CityComboBox.IsEnabled = false;
                return;
            }

            var cities = _cityRepository.GetByCountryId(selectedCountry.Id);
            foreach (var city in cities)
                Cities.Add(city);

            CityComboBox.ItemsSource = Cities;
            CityComboBox.IsEnabled = Cities.Count > 0;
        }

        private void CityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadStreets();
        }

        private void LoadStreets()
        {
            Streets.Clear();

            var selectedCity = CityComboBox.SelectedItem as City;
            if (selectedCity == null)
            {
                StreetComboBox.ItemsSource = null;
                StreetComboBox.IsEnabled = false;
                return;
            }

            var streets = _streetRepository.GetByCityId(selectedCity.Id);
            foreach (var street in streets)
            {
                Streets.Add(street);
            }

            StreetComboBox.ItemsSource = Streets;
            StreetComboBox.IsEnabled = Streets.Count > 0;
        }

        private void AddStreetSection_Click(object sender, RoutedEventArgs e)
        {
            if (!(StreetComboBox.SelectedItem is Street street)) return;
            if (!int.TryParse(StartNumberTextBox.Text, out int start) || !int.TryParse(EndNumberTextBox.Text, out int end))
            {
                MessageBox.Show("Unesite ispravne brojeve.");
                return;
            }

            if (start >= end)
            {
                MessageBox.Show("Početni broj mora biti manji od krajnjeg.");
                return;
            }

            ParityType? side = (SideComboBox.SelectedItem as ComboBoxItem)?.Tag as ParityType?;

            var section = new StreetSection(start, end, side ?? ParityType.Both, street);

            AddedStreetSections.Add(section);

            StartNumberTextBox.Clear();
            EndNumberTextBox.Clear();
        }

        private void RemoveStreetSection_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var sectionToRemove = button?.Tag as StreetSection;

            if (sectionToRemove != null)
            {
                AddedStreetSections.Remove(sectionToRemove);
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
            var selectedCity = CityComboBox.SelectedItem as City;
            var selectedCountry = CountryComboBox.SelectedItem as Country;

            string validationErrors = ValidateInput(name, description, selectedCity, selectedCountry);

            if (!string.IsNullOrEmpty(validationErrors))
            {
                MessageBox.Show("Molimo popunite sledeće nedostatke:\n\n" + validationErrors, 
                    "Validacija", MessageBoxButton.OK);
                return;
            }

            Neighborhood neighborhood = new Neighborhood(name, description, 0, selectedCity, selectedCountry, _user);

            Neighborhood savedNeighborhood = _neighborhoodRepository.Create(neighborhood);
            
            foreach (var section in AddedStreetSections)
            {
                _streetSectionRepository.Create(section, savedNeighborhood.Id);
            }
            
            foreach (string path in SelectedImagePaths)
            {
                var image = new CommunityHub.Application.Domain.Image(0, path, null, savedNeighborhood.Id);
                _imageRepository.Create(image, savedNeighborhood.Id, null);
            }

            MessageBox.Show($"Kvart '{name}' je uspešno registrovan!", "Uspeh");
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
