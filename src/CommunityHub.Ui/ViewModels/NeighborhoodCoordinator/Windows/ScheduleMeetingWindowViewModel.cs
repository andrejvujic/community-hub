using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.Services.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows;
using CommunityHub.Ui.Resources.Localization;

namespace CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Windows
{
    public class ScheduleMeetingWindowViewModel : INotifyPropertyChanged
    {
        private readonly NeighborhoodMeetingService _meetingService = new();
        private readonly NeighborhoodDto _neighborhood;

        public MeetingTheme? FixedTheme { get; }
        public bool HasFixedTheme => FixedTheme.HasValue;

        public string FixedThemeDisplay => FixedTheme switch
        {
            MeetingTheme.Welcome => TranslationSource.Instance["MeetingTheme_Welcome"],
            MeetingTheme.Motivation => TranslationSource.Instance["MeetingTheme_Motivation"],
            _ => string.Empty
        };

        public Visibility FixedThemeVisibility => HasFixedTheme ? Visibility.Visible : Visibility.Collapsed;
        public Visibility SelectableThemeVisibility => HasFixedTheme ? Visibility.Collapsed : Visibility.Visible;

        public DateTime MinDate { get; } = DateTime.Today.AddDays(2);
        public DateTime MaxDate { get; } = DateTime.Today.AddDays(8);

        public bool IsEnglish => TranslationSource.Instance.CurrentCulture?.Name
            .StartsWith("en", StringComparison.OrdinalIgnoreCase) ?? false;
        public Visibility Time24Visibility => IsEnglish ? Visibility.Collapsed : Visibility.Visible;
        public Visibility Time12Visibility => IsEnglish ? Visibility.Visible : Visibility.Collapsed;

        private string _hour = "20";
        public string Hour
        {
            get => _hour;
            set
            {
                _hour = value;
                OnPropertyChanged();
                SyncFrom24To12();
            }
        }

        private string _minute = "00";
        public string Minute
        {
            get => _minute;
            set
            {
                _minute = value;
                OnPropertyChanged();
                _minute12 = value;
                OnPropertyChanged(nameof(Minute12));
            }
        }

        private string _hour12 = "8";
        public string Hour12
        {
            get => _hour12;
            set
            {
                _hour12 = value;
                OnPropertyChanged();
                SyncFrom12To24();
            }
        }

        private string _minute12 = "00";
        public string Minute12
        {
            get => _minute12;
            set
            {
                _minute12 = value;
                OnPropertyChanged();
                _minute = value;
                OnPropertyChanged(nameof(Minute));
            }
        }

        private bool _isPm = true;
        public bool IsPm
        {
            get => _isPm;
            set
            {
                _isPm = value;
                OnPropertyChanged();
                SyncFrom12To24();
            }
        }

        private bool _isSyncing;

        private void SyncFrom12To24()
        {
            if (_isSyncing) return;
            if (!int.TryParse(_hour12, out var h12) || h12 < 1 || h12 > 12) return;

            _isSyncing = true;
            var h24 = h12 % 12 + (_isPm ? 12 : 0);
            _hour = h24.ToString("00");
            OnPropertyChanged(nameof(Hour));
            _isSyncing = false;
        }

        private void SyncFrom24To12()
        {
            if (_isSyncing) return;
            if (!int.TryParse(_hour, out var h24) || h24 < 0 || h24 > 23) return;

            _isSyncing = true;
            _isPm = h24 >= 12;
            var h12 = h24 % 12;
            _hour12 = (h12 == 0 ? 12 : h12).ToString();
            OnPropertyChanged(nameof(Hour12));
            OnPropertyChanged(nameof(IsPm));
            _isSyncing = false;
        }

        private MeetingTheme _selectedTheme = MeetingTheme.Welcome;
        public MeetingTheme SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                _selectedTheme = value;
                OnPropertyChanged();
            }
        }

        private DateTime _fromDate = DateTime.Today.AddDays(2);
        public DateTime FromDate
        {
            get => _fromDate;
            set
            {
                _fromDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime _toDate = DateTime.Today.AddDays(8);
        public DateTime ToDate
        {
            get => _toDate;
            set
            {
                _toDate = value;
                OnPropertyChanged();
            }
        }

        public NeighborhoodMeetingDto? CreatedMeeting { get; private set; }

        public RelayCommand StartVotingCommand { get; }
        public RelayCommand CancelCommand { get; }

        public ScheduleMeetingWindowViewModel(NeighborhoodDto neighborhood, MeetingTheme? fixedTheme)
        {
            _neighborhood = neighborhood;
            FixedTheme = fixedTheme;
            StartVotingCommand = new RelayCommand(ExecuteStartVoting);
            CancelCommand = new RelayCommand(ExecuteCancel);

            TranslationSource.Instance.PropertyChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(IsEnglish));
                OnPropertyChanged(nameof(Time24Visibility));
                OnPropertyChanged(nameof(Time12Visibility));
                OnPropertyChanged(nameof(FixedThemeDisplay));
            };
        }

        private void ExecuteStartVoting()
        {
            if (IsEnglish && !IsHour12Valid())
            {
                ShowErrorDialog(TranslationSource.Instance["Schedule_InvalidHour12"]);
                return;
            }

            if (!TimeOnly.TryParse($"{Hour}:{Minute}", out var time))
            {
                ShowErrorDialog(TranslationSource.Instance["Schedule_InvalidTime"]);
                return;
            }

            if (FromDate > ToDate)
            {
                ShowErrorDialog(TranslationSource.Instance["Schedule_DateOrderError"]);
                return;
            }

            CreateMeeting(time);
            CloseWindow(true);
        }

        private bool IsHour12Valid()
        {
            if (!int.TryParse(_hour12, out var h12) || h12 < 1 || h12 > 12) return false;
            if (!int.TryParse(_minute12, out var m12) || m12 < 0 || m12 > 59) return false;
            return true;
        }

        private void CreateMeeting(TimeOnly time)
        {
            var dto = new NeighborhoodMeetingDto(
                0, _neighborhood, FixedTheme ?? SelectedTheme, time,
                DateOnly.FromDateTime(FromDate), DateOnly.FromDateTime(ToDate),
                MeetingStatus.InPreparation);

            CreatedMeeting = _meetingService.Create(dto);
        }

        private void ShowErrorDialog(string error)
        {
            var window = System.Windows.Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)
                         ?? System.Windows.Application.Current.MainWindow;
            new InfoDialog(TranslationSource.Instance["Error_Title"], error, window).ShowDialog();
        }

        private void ExecuteCancel() => CloseWindow(false);

        private void CloseWindow(bool result)
        {
            var window = System.Windows.Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
            if (window is ScheduleMeetingWindow view)
            {
                view.DialogResult = result;
                view.Close();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}