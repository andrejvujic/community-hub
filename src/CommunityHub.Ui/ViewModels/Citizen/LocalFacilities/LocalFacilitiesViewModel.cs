using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.Services.Dtos.Neighborhoods.CityFacilities;
using CommunityHub.Application.Services.Neighborhoods.CityFacilities;
using CommunityHub.Application.Services.Users;
using CommunityHub.Ui.Helpers.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

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
        public event EventHandler? AddVoteRequested;
        public event EventHandler? RemoveVoteRequested;
        public event EventHandler? ViewHistoryRequested;
        public event EventHandler? ViewStatisticsRequested;

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

        public ICommand ToggleVoteCommand { get; }
        public ICommand ViewHistoryCommand { get; }
        public ICommand ViewStatisticsCommand { get; }

        public LocalFacilitiesViewModel(long userId)
        {
            _userId = userId;

            ToggleVoteCommand = new RelayCommand<object>(_ =>
            {
                if (SelectedFacility?.VotedByMe == true)
                {
                    RemoveVote();
                    RemoveVoteRequested?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    AddVote();
                    AddVoteRequested?.Invoke(this, EventArgs.Empty);
                }
            });
            ViewHistoryCommand = new RelayCommand<object>(_ => ViewHistoryRequested?.Invoke(this, EventArgs.Empty));
            ViewStatisticsCommand = new RelayCommand<object>(_ => ViewStatisticsRequested?.Invoke(this, EventArgs.Empty));

            LoadData();
        }

        private void LoadData()
        {
            var selectedFacilityId = _selectedFacility?.Id;
            var citizen = _citizenService.GetCitizenById(_userId);

            if (citizen?.Membership?.Neighborhood.City?.Id == null)
            {
                Membership = null;
                return;
            }

            PopulateFacilityData(citizen.Membership.Neighborhood.City.Id);

            Membership = citizen.Membership;
            SelectedFacility = Facilities.FirstOrDefault(f => f.Id == selectedFacilityId) ?? Facilities.FirstOrDefault();
        }

        private void PopulateFacilityData(long cityId)
        {
            var facilities = _facilityService.GetByCityId(cityId);

            foreach (var facility in facilities)
            {
                facility.VoteCount = _voteService.GetVoteCountByFacilityId(facility.Id);
                facility.LastVisitDate = _reservationService.GetLastVisitDateForFacility(facility.Id);
                facility.VotedByMe = _voteService.GetByFacilityIdAndCitizenId(facility.Id, _userId) != null;
            }

            Facilities = new ObservableCollection<CityFacilityDto>(facilities);
        }

        public void RemoveVote()
        {
            if (SelectedFacility == null) return;

            _voteService.DeleteByFacilityIdAndCitizenId(SelectedFacility.Id, _userId);
            LoadData();
        }

        public void AddVote()
        {
            if (SelectedFacility == null) return;

            _voteService.Create(SelectedFacility.Id, _userId);
            LoadData();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


