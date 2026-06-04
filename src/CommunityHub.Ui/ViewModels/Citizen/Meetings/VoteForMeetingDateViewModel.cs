using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Ui.Helpers.Commands;
using System.ComponentModel;
using System.Windows.Input;

namespace CommunityHub.Ui.ViewModels.Citizen.Meetings
{
    public class VoteForMeetingDateViewModel : INotifyPropertyChanged
    {
        private readonly NeighborhoodMeetingDto _meeting;
        private DateTime? _selectedDate;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? ConfirmRequested;
        public event EventHandler? CancelRequested;
        public event EventHandler? CloseRequested;
        public event EventHandler? ToggleCalendarRequested;

        public NeighborhoodMeetingDto Meeting => _meeting;

        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate == value) return;
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

        public DateTime DisplayDateStart => _meeting.DateRangeStart.ToDateTime(TimeOnly.MinValue);
        public DateTime DisplayDateEnd => _meeting.DateRangeEnd.ToDateTime(TimeOnly.MinValue);

        public DateOnly SelectedDateAsDateOnly => _selectedDate.HasValue 
            ? DateOnly.FromDateTime(_selectedDate.Value) 
            : DateOnly.MinValue;

        public bool CanConfirm => SelectedDate.HasValue;

        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand ToggleCalendarCommand { get; }

        public VoteForMeetingDateViewModel(NeighborhoodMeetingDto meeting)
        {
            _meeting = meeting;
            ConfirmCommand = new RelayCommand<object>(_ => RequestConfirm());
            CancelCommand = new RelayCommand<object>(_ => RequestCancel());
            CloseCommand = new RelayCommand<object>(_ => RequestClose());
            ToggleCalendarCommand = new RelayCommand<object>(_ => RequestToggleCalendar());
        }

        public void ParseAndSetDate(string dateText)
        {
            dateText = dateText?.Trim() ?? "";

            if (string.IsNullOrEmpty(dateText))
            {
                SelectedDate = null;
                return;
            }

            if (DateTime.TryParse(dateText, out DateTime parsedDate))
            {
                if (parsedDate >= DisplayDateStart && parsedDate <= DisplayDateEnd)
                {
                    SelectedDate = parsedDate;
                }
                else
                {
                    SelectedDate = null;
                }
            }
            else
            {
                SelectedDate = null;
            }
        }

        public void RequestConfirm()
        {
            if (CanConfirm)
                ConfirmRequested?.Invoke(this, EventArgs.Empty);
        }

        public void RequestCancel()
        {
            CancelRequested?.Invoke(this, EventArgs.Empty);
        }

        public void RequestClose()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        public void RequestToggleCalendar()
        {
            ToggleCalendarRequested?.Invoke(this, EventArgs.Empty);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (propertyName == nameof(SelectedDate))
                OnPropertyChanged(nameof(CanConfirm));
        }
    }
}


