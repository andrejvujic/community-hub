using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.MappersDto.Buildings;
using CommunityHub.Application.Services.Dtos.Buildings;
using CommunityHub.Application.Services.ResidentAssemblies;
using CommunityHub.Ui.Helpers.Commands;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using CommunityHub.Application.Domain.ResidentAssemblies;
using System.Windows.Input;
using CommunityHub.Application.Services.Dtos.ResidentAssemblies;
using CommunityHub.Application.MappersDto.Users;
using CommunityHub.Ui.Helpers.BuildingManagerAlerts;

namespace CommunityHub.Ui.ViewModels.BuildingManager.BuildingPages
{
    public class SchedulingAssemblyPageViewModel : INotifyPropertyChanged
    {
        private readonly BuildingMapper _buildingMapper = new();
        private readonly UserMapper _userMapper = new();
        private readonly AssemblyService _assemblyService = new();
        private readonly AssemblyTopicService _assemblyTopicService = new();
        private readonly ObservableCollection<TopicItemViewModel> _topics = [];

        public User User { get; }
        public Building Building { get; }
        public BuildingDto BuildingDto { get; }

        public SchedulingAssemblyPageViewModel(User user, Building building)
        {
            User = user;
            Building = building;
            BuildingDto = _buildingMapper.MapToDto(building);

            _topics.Add(new TopicItemViewModel());
            _topics.CollectionChanged += Topics_CollectionChanged;
            _topics[0].PropertyChanged += Topic_PropertyChanged;

            AddTopicCommand = new RelayCommand(AddTopic);
            RemoveTopicCommand = new RelayCommand<TopicItemViewModel>(RemoveTopic, CanRemoveTopic);
            CancelCommand = new RelayCommand(Cancel);
            ConfirmCommand = new RelayCommand(Confirm);

            UpdateValidation();
        }

        public string StreetInfo
        {
            get => $"{BuildingDto.Address.Street.Name} {BuildingDto.Address.StreetNumber}";
        }

        private DateTime? _selectedDate;
        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    OnPropertyChanged();
                    UpdateValidation();
                }
            }
        }

        private string? _selectedTime;
        public string? SelectedTime
        {
            get => _selectedTime;
            set
            {
                if (_selectedTime != value)
                {
                    _selectedTime = value;
                    OnPropertyChanged();
                    UpdateValidation();
                }
            }
        }

        private string? _dateTimeErrorMessage;
        public string? DateTimeErrorMessage
        {
            get => _dateTimeErrorMessage;
            private set
            {
                if (_dateTimeErrorMessage != value)
                {
                    _dateTimeErrorMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        private string? _topicsErrorMessage;
        public string? TopicsErrorMessage
        {
            get => _topicsErrorMessage;
            private set
            {
                if (_topicsErrorMessage != value)
                {
                    _topicsErrorMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _canConfirm;
        public bool CanConfirm
        {
            get => _canConfirm;
            private set
            {
                if (_canConfirm != value)
                {
                    _canConfirm = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<TopicItemViewModel> Topics => _topics;

        public ICommand AddTopicCommand { get; }
        public ICommand RemoveTopicCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand ConfirmCommand { get; }

        private void AddTopic()
        {
            _topics.Add(new TopicItemViewModel { CanRemove = true });
            UpdateValidation();
        }
        private bool CanRemoveTopic(TopicItemViewModel? topic)
        {
            return topic != null && topic.CanRemove;
        }
        private void RemoveTopic(TopicItemViewModel? topic)
        {
            if (topic == null)
                return;

            if (_topics.Count == 1)
                return;

            if (!topic.CanRemove)
                return;

            _topics.Remove(topic);
            UpdateValidation();
        }
        private void Cancel()
        {
            var dialogService = new DialogService();

            var confirm = dialogService.ShowConfirmation(
                "Potvrda",
                "Da li ste sigurni da želite da otkažete zakazivanje skupštine? Svi uneseni podaci će biti izgubljeni.");

            if (!confirm)
                return;

            ResetForm();
        }
        private void ResetForm()
        {
            SelectedDate = null;
            SelectedTime = null;
            _topics.Clear();
            _topics.Add(new TopicItemViewModel());
            UpdateValidation();
        }
        private void Confirm()
        {
            var dialogService = new DialogService();

            var confirm = dialogService.ShowConfirmation(
                "Potvrda",
                "Da li ste sigurni da želite da zakažete skupštinu?");

            if (!confirm)
                return;

            CreateAssembly();

            dialogService.ShowInfo("Čestitamo", "Uspešno ste kreirali skupštinu stanara.");
        }
        private void CreateAssembly()
        {
            var assembly = AssemblyDto.Create(Building, SelectedDate!.Value.Date.Add(TimeSpan.ParseExact(SelectedTime!.Trim(), "hh\\:mm", CultureInfo.InvariantCulture)));
            var savedAssembly = _assemblyService.Create(assembly);

            CreateTopics(savedAssembly);
        }
        private void CreateTopics(AssemblyDto savedAssembly)
        {
            foreach (var topic in _topics)
            {
                var assemblyTopic = new AssemblyTopicDto(
                    assemblyId: savedAssembly.Id,
                    authorId: User.Id,
                    description: topic.Text!.Trim(),
                    status: AssemblyTopicStatus.Accepted,
                    author: _userMapper.MapToDto(User),
                    isEnforcedByManager: true,
                    id: 0
                );

                _assemblyTopicService.Create(assemblyTopic);
            }

            ResetForm();
        }
        private void Topics_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (TopicItemViewModel item in e.OldItems)
                    item.PropertyChanged -= Topic_PropertyChanged;
            }

            if (e.NewItems != null)
            {
                foreach (TopicItemViewModel item in e.NewItems)
                    item.PropertyChanged += Topic_PropertyChanged;
            }

            UpdateValidation();
        }
        private void Topic_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TopicItemViewModel.Text))
                UpdateValidation();
        }
        private void UpdateValidation()
        {
            DateTimeErrorMessage = GetDateTimeErrorMessage();
            TopicsErrorMessage = GetTopicsErrorMessage();
            
            CanConfirm = string.IsNullOrWhiteSpace(DateTimeErrorMessage)
                && string.IsNullOrWhiteSpace(TopicsErrorMessage);
        }
        private string? GetDateTimeErrorMessage()
        {
            if (!SelectedDate.HasValue || string.IsNullOrWhiteSpace(SelectedTime))
                return "Unesite datum i vreme.";

            if (!TimeSpan.TryParseExact(SelectedTime.Trim(), "hh\\:mm", CultureInfo.InvariantCulture, out var time))
                return "Vreme mora biti u formatu HH:mm.";

            DateTime scheduled = SelectedDate.Value.Date.Add(time);
            if (scheduled < DateTime.Now)
                return "Datum i vreme ne mogu biti u prošlosti.";

            bool hasOverlap = _assemblyService.HasOverlappingAssembliesForManager(User.Id, scheduled);
            if (hasOverlap)
                return "Već postoji zakazana skupština koja se preklapa sa izabranim terminom.";

            return null;
        }
        private string? GetTopicsErrorMessage()
        {
            foreach (var topic in _topics)
                if (string.IsNullOrWhiteSpace(topic.Text) || string.IsNullOrWhiteSpace(topic.Text.Trim()))
                    return "Svaka tema mora biti popunjena.";
            
            return null;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class TopicItemViewModel : INotifyPropertyChanged
    {
        private string? _text;
        private bool _canRemove;

        public string? Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool CanRemove
        {
            get => _canRemove;
            set
            {
                if (_canRemove != value)
                {
                    _canRemove = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}