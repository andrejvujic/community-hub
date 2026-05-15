using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityHub.Application;
using CommunityHub.Application.Domain.Events;
using CommunityHub.Application.Domain.Events.RepositoryInterfaces;
using CommunityHub.Application.Services.Events;

namespace CommunityHub.Ui.ViewModels.Citizen.Events
{
    public class OrganizeEventViewModel : INotifyPropertyChanged
    {
        private readonly long _userId;
        private readonly CommunityEventService _eventService;

        public event PropertyChangedEventHandler? PropertyChanged;

        public OrganizeEventViewModel(long userId)
        {
            _userId = userId;
            _eventService = new CommunityEventService();
            _items = new ObservableCollection<string>();
        }

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(IsFormValid));
            }
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set
            {
                if (_description == value) return;
                _description = value;
                OnPropertyChanged(nameof(Description));
                OnPropertyChanged(nameof(IsFormValid));
            }
        }

        private DateTime? _selectedDate;
        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate == value) return;
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
                OnPropertyChanged(nameof(IsFormValid));
            }
        }

        private string _hour = string.Empty;
        public string Hour
        {
            get => _hour;
            set
            {
                if (_hour == value) return;
                _hour = value;
                OnPropertyChanged(nameof(Hour));
                OnPropertyChanged(nameof(IsFormValid));
            }
        }

        private string _minute = string.Empty;
        public string Minute
        {
            get => _minute;
            set
            {
                if (_minute == value) return;
                _minute = value;
                OnPropertyChanged(nameof(Minute));
                OnPropertyChanged(nameof(IsFormValid));
            }
        }

        private string _durationHour = string.Empty;
        public string DurationHour
        {
            get => _durationHour;
            set
            {
                if (_durationHour == value) return;
                _durationHour = value;
                OnPropertyChanged(nameof(DurationHour));
                OnPropertyChanged(nameof(IsFormValid));
            }
        }

        private string _durationMinute = string.Empty;
        public string DurationMinute
        {
            get => _durationMinute;
            set
            {
                if (_durationMinute == value) return;
                _durationMinute = value;
                OnPropertyChanged(nameof(DurationMinute));
                OnPropertyChanged(nameof(IsFormValid));
            }
        }

        private string _volunteers = string.Empty;
        public string Volunteers
        {
            get => _volunteers;
            set
            {
                if (_volunteers == value) return;
                _volunteers = value;
                OnPropertyChanged(nameof(Volunteers));
                OnPropertyChanged(nameof(IsFormValid));
            }
        }

        private string _itemInput = string.Empty;
        public string ItemInput
        {
            get => _itemInput;
            set
            {
                if (_itemInput == value) return;
                _itemInput = value;
                OnPropertyChanged(nameof(ItemInput));
            }
        }

        private ObservableCollection<string> _items;
        public ObservableCollection<string> Items
        {
            get => _items;
            private set
            {
                if (_items == value) return;
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public bool IsFormValid =>
            !string.IsNullOrWhiteSpace(Name) &&
            !string.IsNullOrWhiteSpace(Description) &&
            CommunityEvent.TryParseEventStart(SelectedDate, Hour, Minute, out _) &&
            CommunityEvent.TryParseDuration(DurationHour, DurationMinute, out _) &&
            CommunityEvent.TryParseMinVolunteers(Volunteers, out _) &&
            Items.Count > 0;

        public void AddItem()
        {
            var input = ItemInput.Trim();
            if (string.IsNullOrEmpty(input))
                return;

            if (!Items.Contains(input))
            {
                Items.Add(input);
                ItemInput = string.Empty;
                OnPropertyChanged(nameof(IsFormValid));
            }
        }

        public void RemoveItem(string item)
        {
            Items.Remove(item);
            OnPropertyChanged(nameof(IsFormValid));
        }

        public void Submit()
        {
            CommunityEvent.TryParseEventStart(SelectedDate, Hour, Minute, out var eventStart);
            CommunityEvent.TryParseDuration(DurationHour, DurationMinute, out var duration);
            CommunityEvent.TryParseMinVolunteers(Volunteers, out var minVolunteers);

            _eventService.CreateEvent(
                organizerId: _userId,
                name: Name,
                description: Description,
                eventStart: eventStart,
                durationMinutes: duration,
                minVolunteers: minVolunteers,
                itemNames: Items.ToList()
            );
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
