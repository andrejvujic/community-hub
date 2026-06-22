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
using CommunityHub.Ui.Resources.Localization;

namespace CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs
{
    public class StatisticsAndMeetingsTabViewModel : INotifyPropertyChanged
    {
        private readonly NeighborhoodService _neighborhoodService = new();
        private readonly NeighborhoodMeetingService _meetingService = new();
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
            string formattedPct = string.Format(TranslationSource.Instance.CurrentCulture ?? System.Globalization.CultureInfo.CurrentCulture, "{0:F2}", pct);
            return $"({formattedPct}%)";
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
            TranslationSource.Instance.Format("Statistics_WelcomeRecommendation", SelectedNeighborhood?.Name);

        public string MotivationRecommendationText =>
            TranslationSource.Instance.Format("Statistics_MotivationRecommendation", SelectedNeighborhood?.Name);

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
                            TranslationSource.Instance["Meetings_TiedTitle"],
                            TranslationSource.Instance["Meetings_TiedMessage"],
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

            TranslationSource.Instance.PropertyChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(WelcomeRecommendationText));
                OnPropertyChanged(nameof(MotivationRecommendationText));
            };
        }

        public void Initialize(long coordinatorId)
        {
            _coordinatorId = coordinatorId;
            LoadNeighborhoods();
        }

        private Window GetActiveWindow() =>
            System.Windows.Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)
            ?? System.Windows.Application.Current.MainWindow;

        public void SelectNeighborhood(long neighborhoodId)
        {
            var neighborhood = Neighborhoods.FirstOrDefault(n => n.Id == neighborhoodId);
            if (neighborhood != null)
                SelectedNeighborhood = neighborhood;
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

        public void LoadMeetingsAndStatistics()
        {
            if (SelectedNeighborhood is null) return;
            LoadMeetings();
            LoadStatistics();
        }

        public void LoadMeetings()
        {
            if (SelectedNeighborhood is null) return;

            Meetings.Clear();
            SelectedMeeting = null;

            foreach (var m in _meetingService.GetByNeighborhood(SelectedNeighborhood.Id))
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

            foreach (var d in _meetingService.GetTiedDatesForMeeting(SelectedMeeting.Id))
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
                new InfoDialog(TranslationSource.Instance["Success_Title"], TranslationSource.Instance["Meeting_ScheduleSuccess"], owner).ShowDialog();
            }

        }

        private void ExecuteConfirmTiedDate()
        {
            if (SelectedMeeting is null || SelectedTiedDateOption is null) return;

            var owner = GetActiveWindow();
            var confirmDialog = new ConfirmDialog(
                TranslationSource.Instance["Meetings_ConfirmTitle"],
                TranslationSource.Instance["Meetings_ConfirmMessage"],
                owner);

            if (confirmDialog.ShowDialog() != true) return;

            _meetingService.UpdateScheduledDate(SelectedMeeting.Id, SelectedTiedDateOption.Value);
            _meetingService.UpdateStatus(SelectedMeeting.Id, MeetingStatus.Scheduled);
            LoadMeetings();

            new InfoDialog(TranslationSource.Instance["Success_Title"], TranslationSource.Instance["Meetings_ConfirmedSuccess"], owner).ShowDialog();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}