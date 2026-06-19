using CommunityHub.Application.Domain.Events;
using CommunityHub.Application.Services.Events;
using CommunityHub.Ui.Helpers;
using CommunityHub.Ui.Helpers.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.ViewModels.Citizen.Events
{
    public class OrganizeEventViewModel : INotifyPropertyChanged
    {
        private readonly long _userId;
        private readonly CommunityEventService _eventService = new();

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? CloseRequested;
        public event EventHandler? ConfirmRequested;
        public event EventHandler? ToggleCalendarRequested;

        public ICommand CloseCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand AddItemCommand { get; }
        public ICommand ToggleCalendarCommand { get; }
        public ICommand RemoveItemCommand { get; }

        public OrganizeEventViewModel(long userId)
        {
            _userId = userId;
            _items = new ObservableCollection<string>();
            CloseCommand = new RelayCommand<object>(_ => HandleClose());
            SaveCommand = new RelayCommand<object>(_ => ConfirmRequested?.Invoke(this, EventArgs.Empty), _ => IsFormValid);
            AddItemCommand = new RelayCommand<object>(_ => AddItem());
            RemoveItemCommand = new RelayCommand<object>(_ => RemoveItem(), _ => SelectedItem != null);
            ToggleCalendarCommand = new RelayCommand<object>(_ => ToggleCalendarRequested?.Invoke(this, EventArgs.Empty));
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
                OnPropertyChanged(nameof(SaveButtonTooltip));
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
                OnPropertyChanged(nameof(SaveButtonTooltip));
            }
        }

        private string? _selectedItem;
        public string? SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem == value) return;
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
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
                OnPropertyChanged(nameof(SaveButtonTooltip));
            }
        }

        public DateTime DisplayDateStart => DateTime.Today;
        public DateTime DisplayDateEnd => DateTime.Today.AddYears(2);

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
                OnPropertyChanged(nameof(SaveButtonTooltip));
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
                OnPropertyChanged(nameof(SaveButtonTooltip));
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
                OnPropertyChanged(nameof(SaveButtonTooltip));
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
                OnPropertyChanged(nameof(SaveButtonTooltip));
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
                OnPropertyChanged(nameof(SaveButtonTooltip));
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
                OnPropertyChanged(nameof(SaveButtonTooltip));
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

        public string SaveButtonTooltip
        {
            get
            {
                if (!TooltipState.IsEnabled || IsFormValid) return null;

                var problemFields = new List<string>();

                if (string.IsNullOrWhiteSpace(Name)) problemFields.Add("naziv");
                if (string.IsNullOrWhiteSpace(Description)) problemFields.Add("opis");
                if (SelectedDate == null) problemFields.Add("datum");
                if (!CommunityEvent.TryParseEventStart(SelectedDate, Hour, Minute, out _)) problemFields.Add("vreme");
                if (!CommunityEvent.TryParseDuration(DurationHour, DurationMinute, out var duration) || duration <= 0) problemFields.Add("trajanje");
                if (!CommunityEvent.TryParseMinVolunteers(Volunteers, out var volunteers) || volunteers < 1) problemFields.Add("volontere");
                if (Items.Count == 0) problemFields.Add("potrebne stvari");

                if (problemFields.Count == 1) return $"Proverite: {problemFields[0]}";
                else return $"Proverite: {string.Join(", ", problemFields)}";
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
            if (string.IsNullOrEmpty(input)) return;
            var capitalizedInput = char.ToUpper(input[0]) + input.Substring(1).ToLower();

            if (!Items.Contains(capitalizedInput))
            {
                Items.Add(capitalizedInput);
                ItemInput = string.Empty;
                OnPropertyChanged(nameof(IsFormValid));
                OnPropertyChanged(nameof(SaveButtonTooltip));
            }
        }

        public void RemoveItem()
        {
            if (SelectedItem != null)
            {
                Items.Remove(SelectedItem);
                SelectedItem = null;
                OnPropertyChanged(nameof(IsFormValid));
                OnPropertyChanged(nameof(SaveButtonTooltip));
            }
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

        public void ParseAndSetDate(string dateText)
        {
            dateText = dateText?.Trim() ?? "";

            if (string.IsNullOrEmpty(dateText))
            {
                SelectedDate = null;
                return;
            }

            if (DateTime.TryParseExact(dateText, "d.M.yyyy.", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
            {
                if (parsedDate >= DisplayDateStart && parsedDate <= DisplayDateEnd)
                {
                    SelectedDate = parsedDate;
                }
                else
                {
                    SelectedDate = null;
                }
            }
            else
            {
                SelectedDate = null;
            }
        }

        public bool HasData()
        {
            return !string.IsNullOrWhiteSpace(Name) ||
                   !string.IsNullOrWhiteSpace(Description) ||
                   SelectedDate != null ||
                   !string.IsNullOrWhiteSpace(Hour) ||
                   !string.IsNullOrWhiteSpace(Minute) ||
                   !string.IsNullOrWhiteSpace(DurationHour) ||
                   !string.IsNullOrWhiteSpace(DurationMinute) ||
                   !string.IsNullOrWhiteSpace(Volunteers) ||
                   Items.Count > 0;
        }

        private void HandleClose()
        {
            if (!HasData())
            {
                CloseRequested?.Invoke(this, EventArgs.Empty);
                return;
            }

            var result = MessageBox.Show("Da li ste sigurni da želite da odbacite sve unete podatke?\n\nSvi podaci o novom događaju biće izgubljeni.","Potvrda odbacivanja", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                CloseRequested?.Invoke(this, EventArgs.Empty);
            }
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
