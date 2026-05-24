using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using CommunityHub.Application.Domain.Events;
using CommunityHub.Application.Services.Dtos.Events;
using CommunityHub.Application.Services.Events;
using CommunityHub.Ui.Helpers.Commands;

namespace CommunityHub.Ui.ViewModels.Citizen.Events
{
    public class MyEventsViewModel : INotifyPropertyChanged
    {
        private readonly long _userId;
        private readonly CommunityEventService _eventService = new();
        private CommunityEventDto? _selectedOrganizerEvent;
        private CommunityEventDto? _selectedVolunteerEvent;

        public long UserId => _userId;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? CloseRequested;
        public event EventHandler? OrganizeEventRequested;
        public event EventHandler<CommunityEventDto>? OpenMarkVolunteersRequested;

        public MyEventsViewModel(long userId)
        {
            _userId = userId;
            _organizerEvents = new ObservableCollection<CommunityEventDto>();
            _volunteerEvents = new ObservableCollection<CommunityEventDto>();

            CloseCommand = new RelayCommand<object>(_ => RequestClose());
            OrganizeEventCommand = new RelayCommand<object>(_ => RequestOrganizeEvent());
            OpenMarkVolunteersCommand = new RelayCommand<object>(_ => RequestOpenMarkVolunteers());

            LoadEvents();
        }

        private ObservableCollection<CommunityEventDto> _organizerEvents;
        public ObservableCollection<CommunityEventDto> OrganizerEvents
        {
            get => _organizerEvents;
            private set
            {
                if (_organizerEvents == value) return;
                _organizerEvents = value;
                OnPropertyChanged(nameof(OrganizerEvents));
            }
        }

        private ObservableCollection<CommunityEventDto> _volunteerEvents;
        public ObservableCollection<CommunityEventDto> VolunteerEvents
        {
            get => _volunteerEvents;
            private set
            {
                if (_volunteerEvents == value) return;
                _volunteerEvents = value;
                OnPropertyChanged(nameof(VolunteerEvents));
            }
        }

        public CommunityEventDto? SelectedOrganizerEvent
        {
            get => _selectedOrganizerEvent;
            set
            {
                if (_selectedOrganizerEvent == value) return;
                _selectedOrganizerEvent = value;
                OnPropertyChanged(nameof(SelectedOrganizerEvent));
            }
        }

        public CommunityEventDto? SelectedVolunteerEvent
        {
            get => _selectedVolunteerEvent;
            set
            {
                if (_selectedVolunteerEvent == value) return;
                _selectedVolunteerEvent = value;
                OnPropertyChanged(nameof(SelectedVolunteerEvent));
            }
        }

        public ICommand CloseCommand { get; }
        public ICommand OrganizeEventCommand { get; }
        public ICommand OpenMarkVolunteersCommand { get; }

        public void LoadEvents()
        {
            var organizerEvents = _eventService.GetByOrganizerId(_userId);
            OrganizerEvents = new ObservableCollection<CommunityEventDto>(organizerEvents);

            var volunteerEvents = _eventService.GetByVolunteerCitizenId(_userId);
            VolunteerEvents = new ObservableCollection<CommunityEventDto>(volunteerEvents);
        }

        public void RequestClose()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        public void SelectFirstEvent()
        {
            if (OrganizerEvents.Count > 0)
            {
                SelectedOrganizerEvent = OrganizerEvents[0];
            }
            else if (VolunteerEvents.Count > 0)
            {
                SelectedVolunteerEvent = VolunteerEvents[0];
            }
        }

        public void RequestOrganizeEvent()
        {
            OrganizeEventRequested?.Invoke(this, EventArgs.Empty);
        }

        public void RequestOpenMarkVolunteers()
        {
            var selectedEvent = SelectedOrganizerEvent;
            if (selectedEvent == null || !CanOpenMarkVolunteersWindow(selectedEvent))
                return;

            var eventDto = GetEventDto(selectedEvent.Id);
            if (eventDto != null)
                OpenMarkVolunteersRequested?.Invoke(this, eventDto);
        }

        public bool CanOpenMarkVolunteersWindow(CommunityEventDto? eventDto)
        {
            if (eventDto == null)
                return false;

            var domainEvent = _eventService.GetById(eventDto.Id);
            if (domainEvent == null)
                return false;

            return domainEvent.Status == EventStatus.Finished && domainEvent.Organizer.Id == _userId;
        }

        public CommunityEventDto? GetEventDto(long eventId)
        {
            return _eventService.GetById(eventId);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
