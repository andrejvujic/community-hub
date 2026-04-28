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
using CommunityHub.Application.Services;

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
        private readonly FacilityReservationRepository _facilityReservationRepository = new FacilityReservationRepository();
        private readonly FacilityBookingService _facilityBookingService = new FacilityBookingService(new FacilityReservationRepository());

        public ObservableCollection<Neighborhood> Neighborhoods { get; set; } = new ObservableCollection<Neighborhood>();
        public ObservableCollection<CityFacility> Facilities { get; set; } = new ObservableCollection<CityFacility>();
        public ObservableCollection<FacilityReservation> Reservations { get; set; } = new ObservableCollection<FacilityReservation>();

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
            LoadReservations();

            FacilitiesPanel.Visibility = Visibility.Visible;
        }

        private void LoadFacilities()
        {
            Facilities.Clear();
            var facilities = _cityFacilityRepository.GetByCityId(_selectedNeighborhood!.City.Id);
            foreach (var f in facilities)
                Facilities.Add(f);
        }

        private void LoadReservations()
        {
            Reservations.Clear();
            var reservations = _facilityReservationRepository.GetByNeighborhoodId(_selectedNeighborhood!.Id);
            foreach (var r in reservations)
                Reservations.Add(r);
        }

        private void ReserveButton_Click(object sender, RoutedEventArgs e)
        {
            var facilityBookingWindow = new FacilityBookingWindow()
            {
                Owner = Window.GetWindow(this)
            };
            facilityBookingWindow.ShowDialog();

        }
    }
}
