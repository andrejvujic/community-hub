using CommunityHub.Application.Services.Neighborhoods.CityFacilities;
using CommunityHub.Application.Services.Dtos.Neighborhoods.CityFacilities;
using CommunityHub.Application.Services.Users;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CommunityHub.Ui.ViewModels.Citizen.LocalFacilities
{
    public class LocalFacilitiesViewModel : INotifyPropertyChanged
    {
        private readonly long _userId;
        private readonly CitizenService _citizenService = new();
        private readonly CityFacilityService _facilityService = new();
        private readonly FacilityVoteService _voteService = new();
        private readonly FacilityReservationService _reservationService = new();

        private ObservableCollection<CityFacilityDto> _facilities = new();
        private CityFacilityDto? _selectedFacility;
        private object? _membership;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<CityFacilityDto> Facilities
        {
            get => _facilities;
            private set
            {
                if (_facilities == value) return;
                _facilities = value;
                OnPropertyChanged(nameof(Facilities));
            }
        }

        public CityFacilityDto? SelectedFacility
        {
            get => _selectedFacility;
            set
            {
                if (_selectedFacility == value) return;
                _selectedFacility = value;
                OnPropertyChanged(nameof(SelectedFacility));
            }
        }

        public object? Membership
        {
            get => _membership;
            private set
            {
                if (_membership == value) return;
                _membership = value;
                OnPropertyChanged(nameof(Membership));
            }
        }

        public LocalFacilitiesViewModel(long userId)
        {
            _userId = userId;
            LoadData();
        }

        private void LoadData()
        {
            var citizen = _citizenService.GetCitizenById(_userId);

            if (citizen?.Membership?.Neighborhood.City?.Id == null)
            {
                Facilities = new ObservableCollection<CityFacilityDto>();
                Membership = null;
                return;
            }

            var facilities = _facilityService.GetByCityId(citizen.Membership.Neighborhood.City.Id);
            PopulateFacilityData(facilities);

            Facilities = new ObservableCollection<CityFacilityDto>(facilities);
            Membership = citizen.Membership;

            if (Facilities.Count > 0)
            {
                SelectedFacility = Facilities[0];
            }
        }

        private void PopulateFacilityData(List<CityFacilityDto> facilities)
        {
            foreach (var facility in facilities)
            {
                facility.VoteCount = _voteService.GetVoteCountByFacilityId(facility.Id);
                facility.LastVisitDate = _reservationService.GetLastVisitDateForFacility(facility.Id);
                facility.VotedByMe = _voteService.GetByFacilityIdAndCitizenId(facility.Id, _userId) != null;
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


