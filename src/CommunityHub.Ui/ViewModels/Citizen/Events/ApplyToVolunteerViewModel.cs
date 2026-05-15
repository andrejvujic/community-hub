using CommunityHub.Application.Services.Dtos.Events;
using CommunityHub.Application.Services.Dtos.Users;
using CommunityHub.Application.Services.Events;
using CommunityHub.Application.Services.Users;
using CommunityHub.Application.Domain.Events;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CommunityHub.Ui.ViewModels.Citizen.Events
{
    public class ApplyToVolunteerViewModel : INotifyPropertyChanged
    {
        private readonly long _userId;
        private readonly long _eventId;
        private readonly CommunityEventService _eventService = new();
        private readonly EventRegistrationService _registrationService = new();
        private readonly EventItemAssignmentService _assignmentService = new();
        private readonly CitizenService _citizenService = new();

        private CommunityEventDto? _event;
        private CitizenDto? _citizen;

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

        public string EventName
        {
            get => _event?.Name ?? string.Empty;
        }

        public DateTime EventStart
        {
            get => _event?.EventStart ?? DateTime.MinValue;
        }

        public ObservableCollection<EventItemSelection> Items { get; } = new();
        public List<long> AlreadyAssignedItemIds { get; } = new();

        public ApplyToVolunteerViewModel(long userId, long eventId)
        {
            _userId = userId;
            _eventId = eventId;
            LoadData();
        }

        private void LoadData()
        {
            _event = _eventService.GetById(_eventId);
            Citizen = _citizenService.GetCitizenById(_userId);

            PopulateItems();
            OnPropertyChanged(nameof(EventName));
            OnPropertyChanged(nameof(EventStart));
        }

        private void PopulateItems()
        {
            _event?.Items?.ForEach(item => Items.Add(new EventItemSelection(item)));

            foreach (var id in _assignmentService.GetByEventId(_eventId).Select(a => a.Item.Id))
                AlreadyAssignedItemIds.Add(id);
        }

        public void Register()
        {
            var registration = _registrationService.CreateRegistration(_citizen!.Id, _eventId);
            AssignSelectedItems(registration.Id);
            _eventService.UpdateEventStatusIfReady(_eventId);
        }

        private void AssignSelectedItems(long registrationId)
        {
            foreach (var selection in Items.Where(s => s.IsChecked))
                _assignmentService.CreateAssignment(registrationId, selection.Item.Id, _eventId);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class EventItemSelection
    {
        public EventItemDto Item { get; }
        public bool IsChecked { get; set; }

        public EventItemSelection(EventItemDto item)
        {
            Item = item;
            IsChecked = false;
        }
    }
}
