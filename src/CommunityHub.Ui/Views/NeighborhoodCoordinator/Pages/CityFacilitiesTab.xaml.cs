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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.CityFacility;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Pages
{
    /// <summary>
    /// Interaction logic for CityFacilitiesTab.xaml
    /// </summary>
    public partial class CityFacilitiesTab : UserControl
    {
        private long _coordinatorId;
        private Neighborhood? _selectedNeighborhood;

        private readonly NeighborhoodRepository _neighborhoodRepository = new NeighborhoodRepository(); 
        private readonly CityFacilityRepository _cityFacilityRepository = new CityFacilityRepository();

        public ObservableCollection<Neighborhood> Neighborhoods { get; set; } = new ObservableCollection<Neighborhood>();
        public ObservableCollection<CityFacility> Facilities { get; set; } = new ObservableCollection<CityFacility>();

        public CityFacilitiesTab()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void Initialize(long coordinatorId)
        {
            _coordinatorId = coordinatorId;
            LoadNeighborhoods();
        }

        private void LoadNeighborhoods()
        {
            Neighborhoods.Clear();
            var neighborhoods = _neighborhoodRepository.GetByCoordinatorId(_coordinatorId);
            foreach (var n in neighborhoods)
                Neighborhoods.Add(n);
        }

        private void NeighborhoodsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedNeighborhood = NeighborhoodsListBox.SelectedItem as Neighborhood;
            if (_selectedNeighborhood is null) return;

            NeighborhoodNameTextBlock.Text = _selectedNeighborhood.Name;
            ReservationsTitleRun.Text = _selectedNeighborhood.Name;

            LoadFacilities();

            FacilitiesPanel.Visibility = Visibility.Visible;
        }

        private void LoadFacilities()
        {
            Facilities.Clear();
            var facilities = _cityFacilityRepository.GetByCityId(_selectedNeighborhood!.City.Id);
            foreach (var f in facilities)
                Facilities.Add(f);
        }
    }
}
