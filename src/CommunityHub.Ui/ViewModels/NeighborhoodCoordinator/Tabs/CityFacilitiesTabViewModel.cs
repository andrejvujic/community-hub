using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Neighborhoods.CityFacilities;
using CommunityHub.Application.Services.Neighborhoods;
using CommunityHub.Application.Services.Neighborhoods.CityFacilities;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows;

namespace CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs
{
    public class CityFacilitiesTabViewModel : INotifyPropertyChanged
    {
        private readonly NeighborhoodService _neighborhoodService = new();
        private readonly CityFacilityService _facilityService = new();
        private readonly FacilityReservationService _reservationService = new();
        private readonly FacilityVoteService _voteService = new();
        private long _coordinatorId;

        public ObservableCollection<NeighborhoodDto> Neighborhoods { get; } = new();
        public ObservableCollection<CityFacilityDto> Facilities { get; } = new();
        public ObservableCollection<FacilityReservationDto> Reservations { get; } = new();

        public RelayCommand GenerateReportCommand { get; }
        public RelayCommand ReserveCommand { get; }

        public DateTime MinDate => DateTime.Today.AddDays(1);

        private string _durationInput = string.Empty;
        public string DurationInput 
        { 
            get => _durationInput; 
            set 
            { 
                _durationInput = value; 
                OnPropertyChanged(); 
            } 
        }

        private DateTime? _startDate;
        public DateTime? StartDate 
        { 
            get => _startDate; 
            set 
            { 
                _startDate = value; 
                OnPropertyChanged(); 
            } 
        }

        private DateTime? _endDate;
        public DateTime? EndDate 
        { 
            get => _endDate; 
            set 
            { _endDate = value; 
                OnPropertyChanged(); 
            } 
        }

        private NeighborhoodDto? _selectedNeighborhood;
        public NeighborhoodDto? SelectedNeighborhood
        {
            get => _selectedNeighborhood;
            set
            {
                if (_selectedNeighborhood == value) return;
                _selectedNeighborhood = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNeighborhoodSelected));
                OnPropertyChanged(nameof(ReservationsTitle));
                LoadFacilities();
                LoadReservations();
            }
        }

        private CityFacilityDto? _selectedFacility;
        public CityFacilityDto? SelectedFacility
        {
            get => _selectedFacility;
            set
            {
                if (_selectedFacility == value) return;
                _selectedFacility = value;
                OnPropertyChanged();
            }
        }

        public bool IsNeighborhoodSelected => SelectedNeighborhood != null;
        public string ReservationsTitle => $"Spisak rezervacija kvarta {SelectedNeighborhood?.Name}";

        public CityFacilitiesTabViewModel()
        {
            GenerateReportCommand = new RelayCommand(ExecuteGenerateReport);
            ReserveCommand = new RelayCommand(ExecuteReserve);
        }

        public void Initialize(long coordinatorId)
        {
            _coordinatorId = coordinatorId;
            StartDate = MinDate;
            EndDate = MinDate;
            LoadNeighborhoods();
        }

        public void LoadNeighborhoods()
        {
            var previously = SelectedNeighborhood;
            Neighborhoods.Clear();

            var neighborhoods = _neighborhoodService.GetByCoordinator(_coordinatorId);
            foreach (var n in neighborhoods)
                Neighborhoods.Add(n);

            SelectedNeighborhood = previously != null
                ? Neighborhoods.FirstOrDefault(n => n.Id == previously.Id)
                : null;
        }

        private void LoadFacilities()
        {
            Facilities.Clear();
            SelectedFacility = null;
            if (SelectedNeighborhood is null) return;

            var facilities = _facilityService.GetByCityId(SelectedNeighborhood.City.Id);
            foreach (var f in facilities)
                f.VoteCount = _voteService.GetVoteCountByFacilityAndNeighborhoodId(f.Id, SelectedNeighborhood.Id);

            foreach (var f in facilities.OrderByDescending(f => f.VoteCount))
                Facilities.Add(f);
        }

        private void LoadReservations()
        {
            Reservations.Clear();
            if (SelectedNeighborhood is null) return;

            foreach (var r in _reservationService.GetByNeighborhoodId(SelectedNeighborhood.Id)
                .OrderByDescending(r => r.StartDatetime))
                Reservations.Add(r);
        }

        private void ExecuteReserve()
        {
            var window = System.Windows.Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)
                ?? System.Windows.Application.Current.MainWindow;

            if (!TryValidateInputs(window, out int duration, out DateTime start, out DateTime end))
                return;

            try
            {
                var result = FindSlots(duration, start, end);

                if (result is null || (result.Value.SlotInRange is null && result.Value.SlotsOutsideRange.Count == 0))
                {
                    new InfoDialog("Nema slobodnih termina", "Nije pronađen nijedan slobodan termin.", window).ShowDialog();
                    return;
                }

                OpenBookingWindow(window, result.Value);
            }
            catch (ArgumentException ex)
            {
                new InfoDialog("Greška", ex.Message, window).ShowDialog();
            }
        }

        private bool TryValidateInputs(Window win, out int duration, out DateTime start, out DateTime end)
        {
            duration = 0; start = DateTime.MinValue; end = DateTime.MinValue;

            if (SelectedFacility is null) return ShowError(win, "Greška", "Izaberite objekat.");

            if (!int.TryParse(DurationInput?.Trim(), out duration) || duration <= 0)
                return ShowError(win, "Greška", "Unesite ispravno trajanje u satima.");

            if (duration > 14) return ShowError(win, "Greška", "Nije moguće rezervisati termin duži od 14h.");

            if (StartDate is null || EndDate is null) return ShowError(win, "Greška", "Izaberite opseg datuma.");

            start = StartDate.Value;
            end = EndDate.Value.AddDays(1).AddSeconds(-1);

            return start < end || ShowError(win, "Greška", "Početni datum mora biti pre krajnjeg.");
        }

        private void OpenBookingWindow(Window owner, (AvailableFacilitySlotDto? SlotInRange, List<AvailableFacilitySlotDto> SlotsOutsideRange) data)
        {
            var dialog = new FacilityBookingWindow(SelectedFacility!, SelectedNeighborhood!, data.SlotInRange, data.SlotsOutsideRange, this)
            {
                Owner = owner
            };
            dialog.ShowDialog();
        }

        private bool ShowError(Window win, string title, string msg)
        {
            new InfoDialog(title, msg, win).ShowDialog();
            return false;
        }

        public (AvailableFacilitySlotDto? SlotInRange, List<AvailableFacilitySlotDto> SlotsOutsideRange)? FindSlots(int durationHours, DateTime rangeStart, DateTime rangeEnd)
        {
            if (SelectedFacility is null) return null;
            return _reservationService.FindSlots(SelectedFacility, durationHours, rangeStart, rangeEnd);
        }

        public FacilityReservationDto Book(AvailableFacilitySlotDto slot)
        {
            var created = _reservationService.Book(SelectedFacility!, SelectedNeighborhood!, slot);
            _voteService.DeleteAllByFacilityAndNeighborhoodId(SelectedFacility!.Id, SelectedNeighborhood!.Id);
            LoadReservations();
            LoadFacilities();
            return created;
        }

        private void ExecuteGenerateReport() { }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}