using System.ComponentModel;
using CommunityHub.Application.Services.Dtos.Events;
using CommunityHub.Application.Services.Events;

namespace CommunityHub.Ui.ViewModels.Citizen.Events
{
    public class MarkVolunteersViewModel : INotifyPropertyChanged
    {
        private readonly CommunityEventService _eventService = new();
        private CommunityEventDto? _event;

        public event PropertyChangedEventHandler? PropertyChanged;

        public MarkVolunteersViewModel(CommunityEventDto eventDto)
        {
            _event = eventDto;
        }

        public CommunityEventDto? Event
        {
            get => _event;
            set
            {
                if (_event == value) return;
                _event = value;
                OnPropertyChanged(nameof(Event));
            }
        }

        public void UpdateAttendance(long registrationId, bool attended)
        {
            _eventService.UpdateEventRegistrationAttendance(registrationId, attended);
        }

        public void RefreshEvent()
        {
            if (_event == null) return;
            var updated = _eventService.GetById(_event.Id);
            if (updated != null)
            {
                Event = updated;
            }
        }

        public void MarkUnattendedAsAbsent()
        {
            if (_event?.Registrations == null) return;

            var unattendedRegistrations = _event.Registrations.Where(r => r.Attended == null).ToList();

            foreach (var reg in unattendedRegistrations)
            {
                UpdateAttendance(reg.Id, false);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
