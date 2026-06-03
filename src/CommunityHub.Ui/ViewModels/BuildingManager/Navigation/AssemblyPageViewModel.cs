using CommunityHub.Application.Domain.ResidentAssemblies;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.Services.Apartments;
using CommunityHub.Application.Services.Buildings;
using CommunityHub.Application.Services.Dtos.Apartments;
using CommunityHub.Application.Services.Dtos.Buildings;
using CommunityHub.Application.Services.Dtos.ResidentAssemblies;
using CommunityHub.Application.Services.ResidentAssemblies;
using CommunityHub.Ui.Helpers.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CommunityHub.Ui.ViewModels.BuildingManager.Navigation
{
    public class AssemblyPageViewModel : INotifyPropertyChanged
    {
        private readonly AssemblyService _assemblyService;
        private readonly BuildingService _buildingService;
        private readonly AssemblyTopicService _assemblyTopicService;
        private readonly AssemblyAttendanceService _assemblyAttendanceService;
        private readonly ApartmentService _apartmentService;

        public User User { get; }
        public ObservableCollection<AssemblyRowViewModel> Assemblies { get; } = [];
        public ObservableCollection<AssemblyTopicDto> Topics { get; } = [];
        public ObservableCollection<ApartmentAttendanceRowViewModel> Apartments { get; } = [];
        public ObservableCollection<AssemblyAttendanceDto> Attendances { get; } = [];
        public ObservableCollection<SuggestedTopicsViewModel> SuggestedTopics { get; } = [];
        public ICommand ConfirmCommand { get; }

        public AssemblyPageViewModel(User user)
        {
            User = user;
            _assemblyService = new();
            _buildingService = new();
            _assemblyTopicService = new();
            _apartmentService = new();
            _assemblyAttendanceService = new();
            ConfirmCommand = new RelayCommand(Confirm);

            _assemblyService.RefreshStatusForAllAssemblies();
            LoadAssemblies();
        }

        public bool IsTopicChangeLocked =>
            SelectedAssembly != null &&
            (SelectedAssembly.Status == AssemblyStatus.Cancelled || DateTime.Now >= SelectedAssembly.Assembly.StartsAt.AddHours(-24));
        public bool CanEditSuggestedTopics => !IsTopicChangeLocked;
        public bool HasAnySuggestedTopics =>
            SuggestedTopics.Any();

        public bool HasAnyAcceptedSuggestedTopics =>
            SuggestedTopics.Any(t => t.Topic.Status == AssemblyTopicStatus.Accepted);

        private AssemblyRowViewModel? _selectedAssembly;
        public AssemblyRowViewModel? SelectedAssembly
        {
            get => _selectedAssembly;
            set
            {
                if (_selectedAssembly != value)
                {
                    _selectedAssembly = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsAssemblySelected));
                    OnPropertyChanged(nameof(IsTopicChangeLocked));
                    OnPropertyChanged(nameof(CanEditSuggestedTopics));

                    LoadTopicsForSelectedAssembly();
                    LoadAttendancesForSelectedAssembly();
                    LoadApartmentsForSelectedAssembly();
                    LoadSuggestedTopics();
                    ApplySuggestedTopicsDeadline();
                }
            }
        }
        public bool IsAssemblySelected => SelectedAssembly != null;

        public void LoadAssemblies()
        {
            Assemblies.Clear();

            var managerBuildings = _buildingService.GetByManagerId(User.Id);
            var buildingLookup = managerBuildings.ToDictionary(building => building.Id);

            var assemblies = managerBuildings
                .SelectMany(building => _assemblyService.GetAllByBuilding(building.Id))
                .OrderByDescending(assembly => assembly.StartsAt)
                .ToList();

            foreach (var assembly in assemblies)
            {
                if (!buildingLookup.TryGetValue(assembly.BuildingId, out var building))
                    continue;

                var topics = _assemblyTopicService.GetAllByAssembly(assembly.Id, AssemblyTopicStatus.Accepted);

                Assemblies.Add(new AssemblyRowViewModel(assembly, building, topics.Count));
            }
        }
        public void LoadTopicsForSelectedAssembly()
        {
            Topics.Clear();

            if (SelectedAssembly == null)
                return;

            var topics = _assemblyTopicService.GetAllByAssembly(SelectedAssembly.Assembly.Id, AssemblyTopicStatus.Accepted);

            foreach (var topic in topics)
            {
                Topics.Add(topic);
            }
        }
        public void LoadApartmentsForSelectedAssembly()
        {
            Apartments.Clear();
            if (SelectedAssembly == null)
                return;

            var apartments = _apartmentService.GetByBuildingId(SelectedAssembly.Assembly.BuildingId);

            var attendanceApartmentIds = Attendances.Select(attendance => attendance.ApartmentId).ToHashSet();

            foreach (var apartment in apartments)
            {
                Apartments.Add(new ApartmentAttendanceRowViewModel
                {
                    Apartment = apartment,
                    HasAttendance = attendanceApartmentIds.Contains(apartment.Id)
                });
            }
        }
        public void LoadAttendancesForSelectedAssembly()
        {
            Attendances.Clear();

            if (SelectedAssembly == null)
                return;

            var attendances = _assemblyAttendanceService.GetAllByAssembly(SelectedAssembly.Assembly.Id);

            foreach (var attendance in attendances)
                Attendances.Add(attendance);
        }
        public bool HasAttendance(ApartmentDto apartment)
        {
            if (apartment == null)
                return false;

            return Attendances.Any(attendance => attendance.ApartmentId == apartment.Id);
        }
        public void LoadSuggestedTopics()
        {
            SuggestedTopics.Clear();

            if (SelectedAssembly == null)
            {
                OnPropertyChanged(nameof(HasAnySuggestedTopics));
                OnPropertyChanged(nameof(HasAnyAcceptedSuggestedTopics));
                return;
            }

            var topics = _assemblyTopicService
                .GetAllByAssembly(SelectedAssembly.Assembly.Id)
                .Where(topic => topic.WasSuggestedByResident)
                .ToList();

            foreach (var topic in topics)
            {
                SuggestedTopics.Add(new SuggestedTopicsViewModel
                {
                    Topic = topic,
                    IsChecked = topic.Status == AssemblyTopicStatus.Accepted
                });
            }

            OnPropertyChanged(nameof(HasAnySuggestedTopics));
            OnPropertyChanged(nameof(HasAnyAcceptedSuggestedTopics));
        }
        public void Confirm()
        {
            if (SelectedAssembly == null || IsTopicChangeLocked)
                return;

            var selectedAssemblyId = SelectedAssembly.Assembly.Id;

            foreach (var suggestedTopic in SuggestedTopics)
            {
                var topic = suggestedTopic.Topic;

                var newStatus = suggestedTopic.IsChecked
                    ? AssemblyTopicStatus.Accepted
                    : AssemblyTopicStatus.Suggested;

                if (topic.Status != newStatus)
                {
                    topic.Status = newStatus;
                    _assemblyTopicService.Update(topic);
                }
            }

            LoadAssemblies();

            SelectedAssembly = Assemblies
                .FirstOrDefault(a => a.Assembly.Id == selectedAssemblyId);
        }

        private void ApplySuggestedTopicsDeadline()
        {
            if (SelectedAssembly == null)
                return;

            RefreshSelectedAssemblyStatus();

            if (SelectedAssembly.Status == AssemblyStatus.Cancelled)
            {
                OnPropertyChanged(nameof(CanEditSuggestedTopics));
                OnPropertyChanged(nameof(IsTopicChangeLocked));
                OnPropertyChanged(nameof(HasAnySuggestedTopics));
                OnPropertyChanged(nameof(HasAnyAcceptedSuggestedTopics));
                return;
            }

            if (DateTime.Now < SelectedAssembly.Assembly.StartsAt.AddHours(-24))
                return;

            if (!SuggestedTopics.Any())
                return;

            if (!SuggestedTopics.Any(t => t.Topic.Status == AssemblyTopicStatus.Accepted))
            {
                var firstTopic = SuggestedTopics.First();
                firstTopic.Topic.Status = AssemblyTopicStatus.Accepted;
                firstTopic.IsChecked = true;
                _assemblyTopicService.Update(firstTopic.Topic);

                LoadTopicsForSelectedAssembly();
                OnPropertyChanged(nameof(HasAnyAcceptedSuggestedTopics));
            }

            OnPropertyChanged(nameof(CanEditSuggestedTopics));
            OnPropertyChanged(nameof(IsTopicChangeLocked));
        }

        private void RefreshSelectedAssemblyStatus()
        {
            if (SelectedAssembly == null)
                return;

            _assemblyService.RefreshStatusForAllAssemblies();

            var freshAssembly = _assemblyService
                .GetAllByBuilding(SelectedAssembly.Assembly.BuildingId)
                .FirstOrDefault(a => a.Id == SelectedAssembly.Assembly.Id);

            if (freshAssembly == null)
                return;

            SelectedAssembly.Status = freshAssembly.Status;

            OnPropertyChanged(nameof(IsTopicChangeLocked));
            OnPropertyChanged(nameof(CanEditSuggestedTopics));
            OnPropertyChanged(nameof(IsAssemblySelected));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class AssemblyRowViewModel : INotifyPropertyChanged
    {
        public AssemblyDto Assembly { get; }
        public BuildingDto? BuildingInfo { get; }

        public string Building => BuildingInfo?.DisplayAddress ?? "";
        public DateTime StartsAt => Assembly.StartsAt;
        public int TopicCount { get; }
        public AssemblyStatus Status
        {
            get => Assembly.Status;
            set
            {
                if (Assembly.Status != value)
                {
                    Assembly.Status = value;
                    OnPropertyChanged();
                }
            }
        }

        public AssemblyRowViewModel(
            AssemblyDto assembly,
            BuildingDto? buildingInfo,
            int topicCount)
        {
            Assembly = assembly;
            BuildingInfo = buildingInfo;
            TopicCount = topicCount;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class ApartmentAttendanceRowViewModel
    {
        public ApartmentDto Apartment { get; set; } = null!;
        public bool HasAttendance { get; set; }
    }
    public class SuggestedTopicsViewModel : INotifyPropertyChanged
    {
        public AssemblyTopicDto Topic { get; set; } = null!;
        public string Description => Topic.Description;

        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}