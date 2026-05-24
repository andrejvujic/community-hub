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
        private EventItemSelection? _selectedItem;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? ConfirmRequested;
        public event EventHandler? CancelRequested;
        public event EventHandler? CloseRequested;

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

        public EventItemSelection? SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem == value) return;
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
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

        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand ToggleItemCommand { get; }

        public ApplyToVolunteerViewModel(long userId, long eventId)
        {
            _userId = userId;
            _eventId = eventId;

            ConfirmCommand = new RelayCommand<object>(_ => RequestConfirm());
            CancelCommand = new RelayCommand<object>(_ => RequestCancel());
            CloseCommand = new RelayCommand<object>(_ => RequestClose());
            ToggleItemCommand = new RelayCommand<object>(_ => ToggleSelectedItem());

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

        public void RequestConfirm()
        {
            Register();
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

        public void ToggleSelectedItem()
        {
            if (SelectedItem != null)
                SelectedItem.IsChecked = !SelectedItem.IsChecked;
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

    public class EventItemSelection : INotifyPropertyChanged
    {
        private bool _isChecked;

        public EventItemDto Item { get; }

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked == value) return;
                _isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public EventItemSelection(EventItemDto item)
        {
            Item = item;
            _isChecked = false;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
