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

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Pages
{
    /// <summary>
    /// Interaction logic for RegisterNeighborhoodWindow.xaml
    /// </summary>
    public partial class RegisterNeighborhoodWindow : Window
    {     
        private readonly long _coordinatorId;


        private readonly CountryRepository _countryRepository = new CountryRepository();
        private readonly CityRepository _cityRepository = new CityRepository();
        private readonly StreetRepository _streetRepository = new StreetRepository();

        public ObservableCollection<Country> Countries { get; }
        public ObservableCollection<City> Cities { get; }
        public ObservableCollection<Street> Streets { get; }

        public RegisterNeighborhoodWindow(long coordinatorId)
        {
            InitializeComponent();
            _coordinatorId = coordinatorId;

            Countries = new ObservableCollection<Country>();
            Cities = new ObservableCollection<City>();
            Streets = new ObservableCollection<Street>();

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

    }
}
