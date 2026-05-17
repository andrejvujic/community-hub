using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodMeetings;
using System.ComponentModel;

namespace CommunityHub.Ui.ViewModels.Citizen.Meetings
{
    public class VoteForMeetingDateViewModel : INotifyPropertyChanged
    {
        private readonly NeighborhoodMeetingDto _meeting;
        private DateTime? _selectedDate;

        public event PropertyChangedEventHandler? PropertyChanged;

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

        public VoteForMeetingDateViewModel(NeighborhoodMeetingDto meeting)
        {
            _meeting = meeting;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (propertyName == nameof(SelectedDate))
                OnPropertyChanged(nameof(CanConfirm));
        }
    }
}

