using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityHub.Application.Domain.Events;
using CommunityHub.Application.Services.Dtos.Events;
using CommunityHub.Application.Services.Events;

namespace CommunityHub.Ui.ViewModels.Citizen.Events
{
    public class MyEventsViewModel : INotifyPropertyChanged
    {
        private readonly long _userId;
        private readonly CommunityEventService _eventService;

        public long UserId => _userId;

        public event PropertyChangedEventHandler? PropertyChanged;

        public MyEventsViewModel(long userId)
        {
            _userId = userId;
            _eventService = new CommunityEventService();
            _organizerEvents = new ObservableCollection<CommunityEventDto>();
            _volunteerEvents = new ObservableCollection<CommunityEventDto>();
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

        public void LoadEvents()
        {
            var organizerEvents = _eventService.GetByOrganizerId(_userId);
            OrganizerEvents = new ObservableCollection<CommunityEventDto>(organizerEvents);

            var volunteerEvents = _eventService.GetByVolunteerCitizenId(_userId);
            VolunteerEvents = new ObservableCollection<CommunityEventDto>(volunteerEvents);
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

        public void RefreshAfterOrganizeWindow()
        {
            LoadEvents();
        }

        public void RefreshAfterMarkVolunteersWindow()
        {
            LoadEvents();
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
