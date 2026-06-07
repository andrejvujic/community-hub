using CommunityHub.Application.Services.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.Services.Users;
using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Ui.Helpers.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace CommunityHub.Ui.ViewModels.Citizen.Meetings
{
    public class MeetingsViewModel : INotifyPropertyChanged
    {
        private readonly long _userId;
        private readonly CitizenService _citizenService = new();
        private readonly NeighborhoodMeetingService _meetingService = new();
        private readonly MeetingDateVoteService _voteService = new();

        private ObservableCollection<NeighborhoodMeetingDto> _meetings = new();
        private NeighborhoodMeetingDto? _selectedMeeting;
        private MeetingDateVoteDto? _currentVote;
        private object? _membership;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? VoteRequested;

        public ObservableCollection<NeighborhoodMeetingDto> Meetings
        {
            get => _meetings;
            private set
            {
                if (_meetings == value) return;
                _meetings = value;
                OnPropertyChanged(nameof(Meetings));
            }
        }

        public NeighborhoodMeetingDto? SelectedMeeting
        {
            get => _selectedMeeting;
            set
            {
                if (_selectedMeeting == value) return;
                _selectedMeeting = value;
                OnPropertyChanged(nameof(SelectedMeeting));
                OnPropertyChanged(nameof(CanVote));
                UpdateCurrentVote();
            }
        }

        public MeetingDateVoteDto? CurrentVote
        {
            get => _currentVote;
            private set
            {
                if (_currentVote == value) return;
                _currentVote = value;
                OnPropertyChanged(nameof(CurrentVote));
                OnPropertyChanged(nameof(HasVoted));
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

        public bool HasVoted => CurrentVote != null;

        public bool CanVote => SelectedMeeting != null && SelectedMeeting.Status == MeetingStatus.InPreparation;

        public ICommand VoteCommand { get; }

        public MeetingsViewModel(long userId)
        {
            _userId = userId;
            VoteCommand = new RelayCommand<object>(_ => VoteRequested?.Invoke(this, EventArgs.Empty));
            LoadData();
        }

        private void LoadData()
        {
            var citizen = _citizenService.GetCitizenById(_userId);

            if (citizen?.Membership?.Neighborhood.Id == null)
            {
                Meetings = new ObservableCollection<NeighborhoodMeetingDto>();
                Membership = null;
                return;
            }

            var neighborhoodId = citizen.Membership.Neighborhood.Id;

            var meetings = _meetingService.GetByNeighborhood(neighborhoodId);
            var voteCounts = meetings.ToDictionary(m => m.Id, m => _voteService.GetVoteCountsForMeeting(m.Id));
            _meetingService.ProcessPendingMeetings(neighborhoodId, voteCounts);

            var updated = _meetingService.GetByNeighborhood(neighborhoodId);
            Meetings = new ObservableCollection<NeighborhoodMeetingDto>(updated);
            Membership = citizen.Membership;

            if (Meetings.Count > 0)
            {
                SelectedMeeting = Meetings[0];
            }
        }

        private void UpdateCurrentVote()
        {
            if (_selectedMeeting == null)
            {
                CurrentVote = null;
                return;
            }

            CurrentVote = _voteService.GetByMeetingIdAndCitizenId(_selectedMeeting.Id, _userId);
        }

        public void ProcessVote(long meetingId, DateOnly votedDate)
        {
            if (CurrentVote != null)
            {
                _voteService.UpdateVote(CurrentVote.Id, votedDate);
            }
            else
            {
                _voteService.CreateVote(meetingId, _userId, votedDate);
            }

            LoadData();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
