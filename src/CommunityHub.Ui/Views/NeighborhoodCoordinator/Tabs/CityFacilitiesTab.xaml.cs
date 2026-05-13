using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain.Neighborhoods;
using CommunityHub.Application.Domain.Neighborhoods.CityFacility;
using CommunityHub.Application.Services;
using CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Tabs
{
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
            if (!TryGetBookingInputs(out var facility, out int durationHours,
                out DateTime rangeStart, out DateTime rangeEnd)) return;

            var (slotInRange, slotsOutsideRange) = _facilityBookingService
                .FindSlots(facility!, durationHours, rangeStart, rangeEnd);

            if (slotInRange is null && slotsOutsideRange.Count == 0)
            {
                MessageBox.Show(
                    "Nije pronađen nijedan slobodan termin u narednih 90 dana.",
                    "Nema slobodnih termina",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            OpenBookingWindow(facility!, slotInRange, slotsOutsideRange);
        }

        private void OpenBookingWindow(
            CityFacility facility,
            AvailableFacilitySlot? slotInRange,
            List<AvailableFacilitySlot> slotsOutsideRange)
        {
            var facilityBookingWindow = new FacilityBookingWindow(facility, _selectedNeighborhood!, slotInRange, slotsOutsideRange, _facilityBookingService)
            {
                Owner = Window.GetWindow(this)
            };

            if (facilityBookingWindow.ShowDialog() == true && facilityBookingWindow.CreatedReservation is not null)
                Reservations.Insert(0, facilityBookingWindow.CreatedReservation);
        }

        private bool TryGetBookingInputs(
            out CityFacility? facility,
            out int durationHours,
            out DateTime rangeStart,
            out DateTime rangeEnd)
        {
            durationHours = 0;
            rangeStart = DateTime.MinValue;
            rangeEnd = DateTime.MinValue;

            facility = FacilitiesComboBox.SelectedItem as CityFacility;
            if (facility is null)
            {
                MessageBox.Show("Izaberite objekat.", "Greška",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!TryGetDuration(out durationHours)) return false;
            if (!TryGetDateRange(out rangeStart, out rangeEnd)) return false;

            return true;
        }

        private bool TryGetDuration(out int durationHours)
        {
            if (!int.TryParse(DurationTextBox.Text.Trim(), out durationHours) || durationHours <= 0)
            {
                MessageBox.Show("Unesite ispravno trajanje posete u satima.", "Greška",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private bool TryGetDateRange(out DateTime rangeStart, out DateTime rangeEnd)
        {
            rangeStart = DateTime.MinValue;
            rangeEnd = DateTime.MinValue;

            if (StartDatePicker.SelectedDate is null || EndDatePicker.SelectedDate is null)
            {
                MessageBox.Show("Izaberite opseg datuma.", "Greška",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            rangeStart = StartDatePicker.SelectedDate.Value;
            rangeEnd = EndDatePicker.SelectedDate.Value.AddDays(1).AddSeconds(-1);

            if (rangeStart >= rangeEnd)
            {
                MessageBox.Show("Početni datum mora biti pre krajnjeg.", "Greška",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
    }
}
