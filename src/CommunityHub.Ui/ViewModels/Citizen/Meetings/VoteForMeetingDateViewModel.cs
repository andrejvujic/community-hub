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

        public DateOnly SelectedDateAsDateOnly => _selectedDate.HasValue ? DateOnly.FromDateTime(_selectedDate.Value) : DateOnly.MinValue;

        public bool CanConfirm => SelectedDate.HasValue;

        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand ToggleCalendarCommand { get; }

        public VoteForMeetingDateViewModel(NeighborhoodMeetingDto meeting)
        {
            _meeting = meeting;
            ConfirmCommand = new RelayCommand<object>(_ => { if (CanConfirm) ConfirmRequested?.Invoke(this, EventArgs.Empty); });
            CancelCommand = new RelayCommand<object>(_ => CancelRequested?.Invoke(this, EventArgs.Empty));
            ToggleCalendarCommand = new RelayCommand<object>(_ => ToggleCalendarRequested?.Invoke(this, EventArgs.Empty));
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

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (propertyName == nameof(SelectedDate)) OnPropertyChanged(nameof(CanConfirm));
        }
    }
}


