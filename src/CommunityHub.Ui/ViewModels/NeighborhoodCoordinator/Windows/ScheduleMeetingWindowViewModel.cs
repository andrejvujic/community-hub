using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.Services.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows;

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
            MeetingTheme.Welcome => "Dobrodošlica",
            MeetingTheme.Motivation => "Motivacija",
            _ => string.Empty
        };

        public Visibility FixedThemeVisibility => HasFixedTheme ? Visibility.Visible : Visibility.Collapsed;
        public Visibility SelectableThemeVisibility => HasFixedTheme ? Visibility.Collapsed : Visibility.Visible;

        public DateTime MinDate { get; } = DateTime.Today.AddDays(2);
        public DateTime MaxDate { get; } = DateTime.Today.AddDays(8);

        private string _hour = "20";
        public string Hour
        {
            get => _hour;
            set 
            { 
                _hour = value; 
                OnPropertyChanged(); 
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
            }
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
        }

        private void ExecuteStartVoting()
        {
            if (!TimeOnly.TryParse($"{Hour}:{Minute}", out var time))
            {
                ShowErrorDialog("Neispravno vreme (00-23h : 00-59m).");
                return;
            }

            if (FromDate > ToDate)
            {
                ShowErrorDialog("Početni datum mora biti pre završnog.");
                return;
            }

            CreateMeeting(time);
            CloseWindow(true);
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
            new InfoDialog("Greška", error, window).ShowDialog();
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