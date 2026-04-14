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

        public ObservableCollection<Country> Countries { get; }
        
        public RegisterNeighborhoodWindow(long coordinatorId)
        {
            InitializeComponent();
            _coordinatorId = coordinatorId;

            Countries = new ObservableCollection<Country>();

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

    }
}
