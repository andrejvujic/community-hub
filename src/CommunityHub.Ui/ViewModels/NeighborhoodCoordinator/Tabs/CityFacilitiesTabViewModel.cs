using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityHub.Application.Domain.Neighborhoods.CityFacilities;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Neighborhoods.CityFacilities;
using CommunityHub.Application.Services.Neighborhoods;
using CommunityHub.Application.Services.Neighborhoods.CityFacilities;
using CommunityHub.Ui.Helpers.Commands;

namespace CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs
{
    public class CityFacilitiesTabViewModel : INotifyPropertyChanged
    {
        private readonly NeighborhoodService _neighborhoodService = new();
        private readonly CityFacilityService _facilityService = new();
        private readonly FacilityReservationService _reservationService = new();
        private readonly FacilityVoteService _voteService = new();
        private readonly long _coordinatorId;

        public ObservableCollection<NeighborhoodDto> Neighborhoods { get; } = new();
        public ObservableCollection<CityFacilityDto> Facilities { get; } = new();
        public ObservableCollection<FacilityReservationDto> Reservations { get; } = new();

        public RelayCommand GenerateReportCommand { get; }

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

        public CityFacilitiesTabViewModel(long coordinatorId)
        {
            _coordinatorId = coordinatorId;
            GenerateReportCommand = new RelayCommand(ExecuteGenerateReport);
        }

        public void LoadNeighborhoods()
        {
            Neighborhoods.Clear();
            foreach (var n in _neighborhoodService.GetByCoordinator(_coordinatorId))
                Neighborhoods.Add(n);
        }

        private void LoadFacilities()
        {
            Facilities.Clear();
            SelectedFacility = null;
            if (SelectedNeighborhood is null) return;

            var facilities = _facilityService.GetByCityId(SelectedNeighborhood.City.Id);
            foreach (var f in facilities)
                f.VoteCount = _voteService.GetVoteCountByFacilityId(f.Id);

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

        public (AvailableFacilitySlotDto? SlotInRange, List<AvailableFacilitySlotDto> SlotsOutsideRange)?
            FindSlots(int durationHours, DateTime rangeStart, DateTime rangeEnd)
        {
            if (SelectedFacility is null) return null;
            return _reservationService.FindSlots(SelectedFacility, durationHours, rangeStart, rangeEnd);
        }

        public FacilityReservationDto Book(AvailableFacilitySlotDto slot)
        {
            var created = _reservationService.Book(SelectedFacility!, SelectedNeighborhood!, slot);
            _voteService.DeleteAllByFacilityId(SelectedFacility!.Id);
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