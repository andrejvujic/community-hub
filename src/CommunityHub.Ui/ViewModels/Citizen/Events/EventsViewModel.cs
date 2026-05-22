using CommunityHub.Application.Services.Dtos.Events;
using CommunityHub.Application.Services.Dtos.Users;
using CommunityHub.Application.Services.Events;
using CommunityHub.Application.Services.Users;
using CommunityHub.Application.Domain.Events;
using CommunityHub.Ui.Helpers.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

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
        private object? _membership;
        private CommunityEventDto? _selectedEvent;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? ApplyToVolunteerRequested;
        public event EventHandler? PrintReportRequested;
        public event EventHandler? MyEventsRequested;

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

        public CommunityEventDto? SelectedEvent
        {
            get => _selectedEvent;
            set
            {
                if (_selectedEvent == value) return;
                _selectedEvent = value;
                OnPropertyChanged(nameof(SelectedEvent));
            }
        }

        public object? Membership
        {
            get => _membership;
            private set
            {
                if (_membership == value) return;
                _membership = value;
                OnPropertyChanged(nameof(Membership));
            }
        }

        public ICommand ApplyToVolunteerCommand { get; }
        public ICommand PrintReportCommand { get; }
        public ICommand MyEventsCommand { get; }

        public EventsViewModel(long userId)
        {
            _userId = userId;

            ApplyToVolunteerCommand = new RelayCommand<object>(_ => ApplyToVolunteerRequested?.Invoke(this, EventArgs.Empty));
            PrintReportCommand = new RelayCommand<object>(_ => PrintReportRequested?.Invoke(this, EventArgs.Empty));
            MyEventsCommand = new RelayCommand<object>(_ => MyEventsRequested?.Invoke(this, EventArgs.Empty));

            LoadData();
        }

        private void LoadData()
        {
            Citizen = _citizenService.GetCitizenById(_userId);

            if (_citizen?.Membership != null)
            {
                RefreshEvents();
                Membership = _citizen.Membership;
            }
            else
            {
                Membership = null;
            }
        }

        public void RefreshEvents()
        {
            _events.Clear();

            if (_citizen?.Membership == null) return;

            var neighborhoodId = _citizen.Membership.Neighborhood.Id;
            var events = _eventService.GetByNeighborhoodId(neighborhoodId);

            foreach (var eventDto in events)
                _events.Add(eventDto);

            if (_events.Count > 0)
            {
                SelectedEvent = _events[0];
            }
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

        public string? ValidateApplyToVolunteer()
        {
            if (SelectedEvent == null)
                return "Izaberite događaj pre prijave.";

            return CanApplyToVolunteer(SelectedEvent.Id);
        }

        public long GetCitizenId() => _citizen?.Id ?? 0;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
