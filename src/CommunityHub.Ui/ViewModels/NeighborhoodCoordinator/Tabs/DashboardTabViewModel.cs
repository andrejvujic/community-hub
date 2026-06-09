using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityHub.Ui.Helpers.Commands;

namespace CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs
{
    public class DashboardTabViewModel : INotifyPropertyChanged
    {
        private long _coordinatorId;

        public ObservableCollection<DashboardNotification> Notifications { get; } = new();

        public RelayCommand ViewRequestsCommand { get; }
        public RelayCommand RegisterNeighborhoodCommand { get; }
        public RelayCommand GenerateReportCommand { get; }

        private int _pendingRequestsCount;
        public int PendingRequestsCount
        {
            get => _pendingRequestsCount;
            private set
            {
                _pendingRequestsCount = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PendingRequestsText));
            }
        }

        public string PendingRequestsText => $"Broj zahteva na čekanju - {PendingRequestsCount}";

        public DashboardTabViewModel()
        {
            ViewRequestsCommand = new RelayCommand(ExecuteViewRequests);
            RegisterNeighborhoodCommand = new RelayCommand(ExecuteRegisterNeighborhood);
            GenerateReportCommand = new RelayCommand(ExecuteGenerateReport);
        }

        public void Initialize(long coordinatorId)
        {
            _coordinatorId = coordinatorId;
            LoadDashboard();
        }

        private void LoadDashboard()
        {
            LoadPendingRequests();
            LoadNotifications();
        }

        private void LoadPendingRequests()
        {
        }

        private void LoadNotifications()
        {
            Notifications.Clear();
        }

        private void ExecuteViewRequests()
        {
        }

        private void ExecuteRegisterNeighborhood()
        {
        }

        private void ExecuteGenerateReport()
        {
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class DashboardNotification
    {
        public string Icon { get; init; } = string.Empty;
        public string TypeLabel { get; init; } = string.Empty;
        public string DateText { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string ActionLabel { get; init; } = string.Empty;
        public RelayCommand? ActionCommand { get; init; }
    }
}