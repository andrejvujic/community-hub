using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.Domain.TrustRecords;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.Services.Neighborhoods;
using CommunityHub.Application.Services.Neighborhoods.NeighborhoodMeetings;

namespace CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs
{
    public class StatisticsAndMeetingsTabViewModel : INotifyPropertyChanged
    {
        private readonly NeighborhoodService _neighborhoodService = new();
        private readonly NeighborhoodMeetingService _meetingService = new();
        private readonly MeetingDateVoteService _voteService = new();
        private readonly NeighborhoodMembershipService _membershipService = new();
        private readonly long _coordinatorId;

        public ObservableCollection<NeighborhoodDto> Neighborhoods { get; } = new();
        public ObservableCollection<NeighborhoodMeetingDto> Meetings { get; } = new();
        public ObservableCollection<TiedDateOption> TiedDateOptions { get; } = new();

        private NeighborhoodDto? _selectedNeighborhood;
        public NeighborhoodDto? SelectedNeighborhood
        {
            get => _selectedNeighborhood;
            set
            {
                if (_selectedNeighborhood == value) return;
                _selectedNeighborhood = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsDetailVisible));
                LoadMeetingsAndStatistics();
            }
        }

        public bool IsDetailVisible => SelectedNeighborhood != null;

        private int _totalCitizens;
        public int TotalCitizens
        {
            get => _totalCitizens;
            private set 
            { 
                _totalCitizens = value; 
                OnPropertyChanged(); 
                OnPropertyChanged(nameof(TotalCitizensText)); 
            }
        }

        private int _newCount;
        public int NewCount
        {
            get => _newCount;
            private set 
            { 
                _newCount = value; 
                OnPropertyChanged(); 
                OnPropertyChanged(nameof(NewCountText)); 
                OnPropertyChanged(nameof(NewPercentText)); 
            }
        }

        private int _inactiveCount;
        public int InactiveCount
        {
            get => _inactiveCount;
            private set 
            { 
                _inactiveCount = value; 
                OnPropertyChanged(); 
                OnPropertyChanged(nameof(InactiveCountText));
                OnPropertyChanged(nameof(InactivePercentText)); 
            }
        }

        private int _activeCount;
        public int ActiveCount
        {
            get => _activeCount;
            private set 
            { 
                _activeCount = value; 
                OnPropertyChanged(); 
                OnPropertyChanged(nameof(ActiveCountText)); 
                OnPropertyChanged(nameof(ActivePercentText)); 
            }
        }

        private int _prominentCount;
        public int ProminentCount
        {
            get => _prominentCount;
            private set 
            { 
                _prominentCount = value; 
                OnPropertyChanged(); 
                OnPropertyChanged(nameof(ProminentCountText)); 
                OnPropertyChanged(nameof(ProminentPercentText));
            }
        }

        private int _trustworthyCount;
        public int TrustworthyCount
        {
            get => _trustworthyCount;
            private set 
            { 
                _trustworthyCount = value; 
                OnPropertyChanged(); 
                OnPropertyChanged(nameof(TrustworthyCountText)); 
                OnPropertyChanged(nameof(TrustworthyPercentText)); 
            }
        }

        public string TotalCitizensText => TotalCitizens.ToString();
        public string NewCountText => NewCount.ToString();
        public string NewPercentText => FormatPercent(NewCount);
        public string InactiveCountText => InactiveCount.ToString();
        public string InactivePercentText => FormatPercent(InactiveCount);
        public string ActiveCountText => ActiveCount.ToString();
        public string ActivePercentText => FormatPercent(ActiveCount);
        public string ProminentCountText => ProminentCount.ToString();
        public string ProminentPercentText => FormatPercent(ProminentCount);
        public string TrustworthyCountText => TrustworthyCount.ToString();
        public string TrustworthyPercentText => FormatPercent(TrustworthyCount);

        private string FormatPercent(int count)
        {
            double pct = TotalCitizens > 0 ? Math.Round((double)count / TotalCitizens * 100, 1) : 0;
            return $"({pct}%)";
        }

        private bool _showWelcomeRecommendation;
        public bool ShowWelcomeRecommendation
        {
            get => _showWelcomeRecommendation;
            private set
            {
                _showWelcomeRecommendation = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ShowAnyRecommendation));
                OnPropertyChanged(nameof(WelcomeRecommendationText));
            }
        }

        private bool _showMotivationRecommendation;
        public bool ShowMotivationRecommendation
        {
            get => _showMotivationRecommendation;
            private set
            {
                _showMotivationRecommendation = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ShowAnyRecommendation));
                OnPropertyChanged(nameof(MotivationRecommendationText));
            }
        }

        public bool ShowAnyRecommendation => ShowWelcomeRecommendation || ShowMotivationRecommendation;

        public string WelcomeRecommendationText =>
            $"U kvartu {SelectedNeighborhood?.Name} je primećeno da je najveći broj novih građana.\n" +
            $"Predlažemo vam da zakažete sastanak dobrodošlice.";

        public string MotivationRecommendationText =>
            $"U kvartu {SelectedNeighborhood?.Name} je primećeno da je najveći broj neaktivnih građana.\n" +
            $"Predlažemo vam da zakažete sastanak motivacije.";

        private NeighborhoodMeetingDto? _selectedMeeting;
        public NeighborhoodMeetingDto? SelectedMeeting
        {
            get => _selectedMeeting;
            set
            {
                if (_selectedMeeting == value) return;
                _selectedMeeting = value;
                OnPropertyChanged();
                LoadTiedDates();
            }
        }

        public bool HasTiedDates => TiedDateOptions.Count > 0;

        private TiedDateOption? _selectedTiedDateOption;
        public TiedDateOption? SelectedTiedDateOption
        {
            get => _selectedTiedDateOption;
            set
            {
                _selectedTiedDateOption = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanConfirmTiedDate));
            }
        }

        public bool CanConfirmTiedDate => SelectedTiedDateOption != null;

        public StatisticsAndMeetingsTabViewModel(long coordinatorId)
        {
            _coordinatorId = coordinatorId;
        }

        public void LoadNeighborhoods()
        {
            Neighborhoods.Clear();
            var neighborhoods = _neighborhoodService.GetByCoordinator(_coordinatorId);
            foreach (var n in neighborhoods)
                Neighborhoods.Add(n);
        }

        private void LoadMeetingsAndStatistics()
        {
            if (SelectedNeighborhood is null) return;
            LoadMeetings();
            LoadStatistics();
        }

        public void LoadMeetings()
        {
            if (SelectedNeighborhood is null) return;

            var meetings = _meetingService.GetByNeighborhood(SelectedNeighborhood.Id);
            ProcessMeetingsInPreparation(meetings);

            var updated = _meetingService.GetByNeighborhood(SelectedNeighborhood.Id);
            Meetings.Clear();
            SelectedMeeting = null;
            foreach (var m in updated)
                Meetings.Add(m);
        }

        private void ProcessMeetingsInPreparation(List<NeighborhoodMeetingDto> meetings)
        {
            foreach (var m in meetings.Where(m => m.IsTied))
                ProcessSingleMeeting(m);
        }

        private void ProcessSingleMeeting(NeighborhoodMeetingDto meeting)
        {
            if (IsExpiredUnresolved(meeting))
            {
                _meetingService.UpdateStatus(meeting.Id, MeetingStatus.Cancelled);
                return;
            }

            var voteCounts = _voteService.GetVoteCountsForMeeting(meeting.Id);

            if (voteCounts.Count == 0)
            {
                _meetingService.UpdateStatus(meeting.Id, MeetingStatus.Cancelled);
                return;
            }

            TryAutoSchedule(meeting, voteCounts);
        }

        /// <summary>
        /// Vraća true ako je prošao trenutak početnog datuma u opsegu,
        /// a sastanak još nije zakazan — koordinator nije odabrao termin na vreme.
        /// </summary>
        private static bool IsExpiredUnresolved(NeighborhoodMeetingDto meeting)
        {
            var startMoment = meeting.DateRangeStart.ToDateTime(meeting.MeetingTime);
            return DateTime.Now >= startMoment;
        }

        private void TryAutoSchedule(NeighborhoodMeetingDto meeting, Dictionary<DateOnly, int> voteCounts)
        {
            var scheduled = NeighborhoodMeeting.ResolveScheduledDate(voteCounts);
            if (!scheduled.HasValue) return; // izjednačeno — koordinator bira

            _meetingService.UpdateScheduledDate(meeting.Id, scheduled.Value);
            _meetingService.UpdateStatus(meeting.Id, MeetingStatus.Scheduled);
        }

        private void LoadStatistics()
        {
            if (SelectedNeighborhood is null) return;

            var records = _membershipService.GetTrustRecordsByNeighborhood(SelectedNeighborhood.Id);
            TotalCitizens = records.Count;
            NewCount = records.Count(r => r.TrustStatus == TrustStatus.New);
            InactiveCount = records.Count(r => r.TrustStatus == TrustStatus.Inactive);
            ActiveCount = records.Count(r => r.TrustStatus == TrustStatus.Active);
            ProminentCount = records.Count(r => r.TrustStatus == TrustStatus.Prominent);
            TrustworthyCount = records.Count(r => r.TrustStatus == TrustStatus.Trustworthy);

            int maxCount = new[] { NewCount, InactiveCount, ActiveCount, ProminentCount, TrustworthyCount }.Max();
            ShowWelcomeRecommendation = maxCount == NewCount && NewCount > 0;
            ShowMotivationRecommendation = !ShowWelcomeRecommendation && maxCount == InactiveCount && InactiveCount > 0;

            OnPropertyChanged(nameof(TotalCitizens));
            OnPropertyChanged(nameof(NewCount));
            OnPropertyChanged(nameof(InactiveCount));
            OnPropertyChanged(nameof(ActiveCount));
            OnPropertyChanged(nameof(ProminentCount));
            OnPropertyChanged(nameof(TrustworthyCount));
            OnPropertyChanged(nameof(ShowWelcomeRecommendation));
            OnPropertyChanged(nameof(ShowMotivationRecommendation));
        }

        private void LoadTiedDates()
        {
            TiedDateOptions.Clear();
            SelectedTiedDateOption = null;
            OnPropertyChanged(nameof(HasTiedDates));

            if (SelectedMeeting is null) return;
            if (SelectedMeeting.Status != MeetingStatus.InPreparation) return;
            if (!SelectedMeeting.IsTied) return;

            var voteCounts = _voteService.GetVoteCountsForMeeting(SelectedMeeting.Id);
            if (!NeighborhoodMeeting.HasTiedDates(voteCounts)) return;

            foreach (var d in NeighborhoodMeeting.GetTiedDates(voteCounts))
                TiedDateOptions.Add(new TiedDateOption(d));

            OnPropertyChanged(nameof(HasTiedDates));
        }

        public void ConfirmTiedDate()
        {
            if (SelectedMeeting is null || SelectedTiedDateOption is null) return;

            _meetingService.UpdateScheduledDate(SelectedMeeting.Id, SelectedTiedDateOption.Date);
            _meetingService.UpdateStatus(SelectedMeeting.Id, MeetingStatus.Scheduled);
            LoadMeetings();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public class TiedDateOption
    {
        public DateOnly Date { get; }
        public string Display => Date.ToString("dd.MM.yyyy.");

        public TiedDateOption(DateOnly date) => Date = date;
    }
}