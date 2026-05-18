using CommunityHub.Application.Services.Dtos.Events;
using CommunityHub.Application.Services.Dtos.Users;
using CommunityHub.Application.Services.Events;
using CommunityHub.Application.Services.Users;
using CommunityHub.Application.Domain.Events;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CommunityHub.Ui.ViewModels.Citizen.Events
{
    public class EventsViewModel : INotifyPropertyChanged
    {
        private readonly long _userId;
        private readonly CommunityEventService _eventService = new();
        private readonly EventRegistrationService _registrationService = new();
        private readonly CitizenService _citizenService = new();

        private CitizenDto? _citizen;
        private ObservableCollection<CommunityEventDto> _events = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        public CitizenDto? Citizen
        {
            get => _citizen;
            private set
            {
                if (_citizen == value) return;
                _citizen = value;
                OnPropertyChanged(nameof(Citizen));
            }
        }

        public ObservableCollection<CommunityEventDto> Events => _events;

        public bool HasMembership
        {
            get => _citizen?.Membership != null;
        }

        public EventsViewModel(long userId)
        {
            _userId = userId;
            LoadData();
        }

        private void LoadData()
        {
            Citizen = _citizenService.GetCitizenById(_userId);

            if (HasMembership)
            {
                RefreshEvents();
            }

            OnPropertyChanged(nameof(HasMembership));
        }

        public void RefreshEvents()
        {
            _events.Clear();

            if (_citizen?.Membership == null) return;

            var neighborhoodId = _citizen.Membership.Neighborhood.Id;
            var events = _eventService.GetByNeighborhoodId(neighborhoodId);

            foreach (var eventDto in events)
                _events.Add(eventDto);
        }

        public string? CanApplyToVolunteer(long selectedEventId)
        {
            var eventDto = _eventService.GetById(selectedEventId);

            if (eventDto!.Status != EventStatus.Preparation)
                return "Prijava je dozvoljena samo dok je događaj u pripremi.";

            if (_registrationService.IsAlreadyRegistered(_citizen!.Id, selectedEventId))
                return "Već ste se prijavili za ovaj događaj.";

            return null;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
