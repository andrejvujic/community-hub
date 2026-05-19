using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.Services.Neighborhoods.NeighborhoodMeetings;

namespace CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Windows
{
    public class ScheduleMeetingWindowViewModel : INotifyPropertyChanged
    {
        private readonly NeighborhoodMeetingService _meetingService = new();
        private readonly NeighborhoodDto _neighborhood;

        public MeetingTheme? FixedTheme { get; }
        public bool HasFixedTheme => FixedTheme.HasValue;
        public bool HasSelectableTheme => !HasFixedTheme;

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
                OnPropertyChanged(nameof(CanSubmit));
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
                OnPropertyChanged(nameof(CanSubmit));
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
                OnPropertyChanged(nameof(CanSubmit));
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
                OnPropertyChanged(nameof(CanSubmit));
            }
        }

        public bool CanSubmit =>
            TimeOnly.TryParse($"{Hour}:{Minute}", out _) && FromDate <= ToDate;

        public ScheduleMeetingWindowViewModel(NeighborhoodDto neighborhood, MeetingTheme? fixedTheme)
        {
            _neighborhood = neighborhood;
            FixedTheme = fixedTheme;
        }

        public NeighborhoodMeetingDto? CreatedMeeting { get; private set; }

        public (bool Success, string? Error) TryCreate()
        {
            if (!TimeOnly.TryParse($"{Hour}:{Minute}", out var time))
                return (false, "Neispravno vreme (00-23h : 00-59m).");

            if (FromDate > ToDate)
                return (false, "Početni datum mora biti pre završnog.");

            var dto = new NeighborhoodMeetingDto(
                0,
                _neighborhood,
                FixedTheme ?? SelectedTheme,
                time,
                DateOnly.FromDateTime(FromDate),
                DateOnly.FromDateTime(ToDate),
                MeetingStatus.InPreparation);

            CreatedMeeting = _meetingService.Create(dto);
            return (true, null);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}