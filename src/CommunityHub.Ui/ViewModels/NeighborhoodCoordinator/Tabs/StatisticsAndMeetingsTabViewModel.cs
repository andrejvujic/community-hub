using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.Domain.TrustRecords;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.Services.Neighborhoods;
using CommunityHub.Application.Services.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows;

namespace CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs
{
    public class StatisticsAndMeetingsTabViewModel : INotifyPropertyChanged
    {
        private readonly NeighborhoodService _neighborhoodService = new();
        private readonly NeighborhoodMeetingService _meetingService = new();
        private readonly MeetingDateVoteService _voteService = new();
        private readonly NeighborhoodMembershipService _membershipService = new();
        private long _coordinatorId;

        public ObservableCollection<NeighborhoodDto> Neighborhoods { get; } = new();
        public ObservableCollection<NeighborhoodMeetingDto> Meetings { get; } = new();
        public ObservableCollection<DateOnly> TiedDateOptions { get; } = new();
        public RelayCommand ScheduleMeetingCommand { get; }
        public RelayCommand ScheduleWelcomeCommand { get; }
        public RelayCommand ScheduleMotivationCommand { get; }
        public RelayCommand ConfirmTiedDateCommand { get; }

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

                if (HasTiedDates)
                {
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(() =>
                    {
                        new InfoDialog(
                            "Izjednačeni termini",
                            "Postoji izjednačen broj glasova. Odaberite jedan od termina.",
                            GetActiveWindow()
                        ).ShowDialog();
                    });
                }
            }
        }

        public bool HasTiedDates => TiedDateOptions.Count > 0;

        private DateOnly? _selectedTiedDateOption;
        public DateOnly? SelectedTiedDateOption
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

        public StatisticsAndMeetingsTabViewModel()
        {
            ScheduleMeetingCommand = new RelayCommand(() => OpenScheduleWindow(null));
            ScheduleWelcomeCommand = new RelayCommand(() => OpenScheduleWindow(MeetingTheme.Welcome));
            ScheduleMotivationCommand = new RelayCommand(() => OpenScheduleWindow(MeetingTheme.Motivation));
            ConfirmTiedDateCommand = new RelayCommand(ExecuteConfirmTiedDate, () => CanConfirmTiedDate);
        }

        public void Initialize(long coordinatorId)
        {
            _coordinatorId = coordinatorId;
            LoadNeighborhoods();
        }

        private Window GetActiveWindow() =>
            System.Windows.Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)
            ?? System.Windows.Application.Current.MainWindow;

        public void LoadNeighborhoods()
        {
            Neighborhoods.Clear();
            foreach (var n in _neighborhoodService.GetByCoordinator(_coordinatorId))
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
            var voteCounts = meetings.ToDictionary(m => m.Id, m => _voteService.GetVoteCountsForMeeting(m.Id));
            _meetingService.ProcessPendingMeetings(SelectedNeighborhood.Id, voteCounts);

            var updated = _meetingService.GetByNeighborhood(SelectedNeighborhood.Id);
            Meetings.Clear();
            SelectedMeeting = null;
            foreach (var m in updated)
                Meetings.Add(m);
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
                TiedDateOptions.Add(d);

            OnPropertyChanged(nameof(HasTiedDates));
        }

        private void OpenScheduleWindow(MeetingTheme? theme)
        {
            if (SelectedNeighborhood is null) return;

            var window = new ScheduleMeetingWindow(SelectedNeighborhood, theme)
            {
                Owner = GetActiveWindow()
            };

            var owner = GetActiveWindow();

            if (window.ShowDialog() == true)
            {
                LoadMeetings();
                new InfoDialog("Uspeh", "Glasanje za novi sastanak je pokrenuto.", owner).ShowDialog();
            }

        }

        private void ExecuteConfirmTiedDate()
        {
            if (SelectedMeeting is null || SelectedTiedDateOption is null) return;

            var owner = GetActiveWindow();

            var confirmDialog = new ConfirmDialog(
                "Potvrda termina",
                "Da li ste sigurni da želite da potvrdite ovaj termin?",
                owner);

            if (confirmDialog.ShowDialog() != true) return;

            _meetingService.UpdateScheduledDate(SelectedMeeting.Id, SelectedTiedDateOption.Value);
            _meetingService.UpdateStatus(SelectedMeeting.Id, MeetingStatus.Scheduled);
            LoadMeetings();

            new InfoDialog("Uspeh", "Termin je uspešno zakazan.", owner).ShowDialog();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}