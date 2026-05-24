using System.ComponentModel;
using System.Windows.Input;
using CommunityHub.Application.Services.Dtos.Events;
using CommunityHub.Application.Services.Events;
using CommunityHub.Ui.Helpers.Commands;

namespace CommunityHub.Ui.ViewModels.Citizen.Events
{
    public class MarkVolunteersViewModel : INotifyPropertyChanged
    {
        private readonly CommunityEventService _eventService = new();
        private CommunityEventDto? _event;
        private EventRegistrationDto? _selectedRegistration;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? CloseRequested;

        public ICommand CloseCommand { get; }
        public ICommand UpdateAttendanceCommand { get; }

        public MarkVolunteersViewModel(CommunityEventDto eventDto)
        {
            _event = eventDto;
            CloseCommand = new RelayCommand<object>(_ => RequestClose());
            UpdateAttendanceCommand = new RelayCommand<EventRegistrationDto>(reg => UpdateAttendanceForRegistration(reg));
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

        public EventRegistrationDto? SelectedRegistration
        {
            get => _selectedRegistration;
            set
            {
                if (_selectedRegistration == value) return;
                _selectedRegistration = value;
                OnPropertyChanged(nameof(SelectedRegistration));
            }
        }

        public void UpdateAttendance(long registrationId, bool attended)
        {
            _eventService.UpdateEventRegistrationAttendance(registrationId, attended);
        }

        private void UpdateAttendanceForRegistration(EventRegistrationDto? registration)
        {
            if (registration == null) return;
            bool newValue = registration.Attended != true;
            UpdateAttendance(registration.Id, newValue);
            RefreshEvent();

            if (_event?.Registrations != null)
            {
                var registrationToSelect = _event.Registrations.FirstOrDefault(r => r.Id == registration.Id);
                if (registrationToSelect != null)
                {
                    SelectedRegistration = registrationToSelect;
                }
            }
        }

        public void RequestClose()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        public void SelectFirstRegistration()
        {
            if (_event?.Registrations?.Count > 0)
            {
                SelectedRegistration = _event.Registrations[0];
            }
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
