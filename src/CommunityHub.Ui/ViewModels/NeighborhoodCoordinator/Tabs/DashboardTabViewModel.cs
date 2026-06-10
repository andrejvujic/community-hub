using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using CommunityHub.Application.Services.Dtos.ServiceBookings;
using CommunityHub.Application.Services.Dtos.Users;
using CommunityHub.Application.Services.JobListingBookings;
using CommunityHub.Application.Services.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.Services.Users;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows;

namespace CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs
{
    public class DashboardTabViewModel : INotifyPropertyChanged
    {
        private readonly BookedJobService _bookedJobService = new();
        private readonly UserService _userService = new();
        private readonly BudgetDonationService _budgetDonationService = new();

        private long _coordinatorId;
        private UserDto? _coordinator;

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
            _coordinator = _userService.Get(_coordinatorId);
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
            AddDonationNotifications();
            AddUnreviewedServiceNotifications();
            AddEditableReviewNotifications();
        }

        private void AddDonationNotifications()
        {
            var donations = _budgetDonationService.GetRecentByCoordinator(_coordinatorId);

            foreach (var donation in donations)
            {
                Notifications.Add(new DashboardNotification
                {
                    Icon = "\uf0d6",
                    TypeLabel = "Donacija građana",
                    DateText = $"Datum: {donation.DonationDate:dd.MM.yyyy.}",
                    Description = $"Izvršena je uplata u iznosu od {donation.Amount:N0} RSD " +
                                  $"za budžet kvarta {donation.Neighborhood.Name} " +
                                  $"(kategorija: {donation.BudgetCategory.Name}).",
                    ActionLabel = "Detalji budžeta (Ctrl + 7)",
                    //ActionCommand = 
                });
            }
        }

        private void AddUnreviewedServiceNotifications()
        {
            if (_coordinator is null) return;
            var unreviewed = _bookedJobService.GetRecentUnreviewedByCoordinator(_coordinatorId);

            foreach (var job in unreviewed)
            {
                var providerName = $"{job.Listing.Provider.Name} {job.Listing.Provider.Surname}".Trim();
                var capturedJob = job;
                Notifications.Add(new DashboardNotification
                {
                    Icon = "\uf7d9",
                    TypeLabel = "Servisna usluga",
                    DateText = $"Datum: {job.ScheduledDate:dd.MM.yyyy.}",
                    Description = $"Serviser '{providerName}' je obavio uslugu \"{job.Listing.ServiceName}\". " +
                                  $"Trošak od {job.TotalPrice:N2} RSD je evidentiran u rashodima. " +
                                  $"Oceniti možete još {FormatReviewDeadline(job)}.",
                    ActionLabel = "Oceni",
                    ActionCommand = new RelayCommand(() => OpenReviewWindow(capturedJob, isEdit: false))
                });
            }
        }

        private void AddEditableReviewNotifications()
        {
            if (_coordinator is null) return;
            var editable = _bookedJobService.GetEditableByCoordinator(_coordinatorId);

            foreach (var job in editable)
            {
                var providerName = $"{job.Listing.Provider.Name} {job.Listing.Provider.Surname}".Trim();
                var capturedJob = job;
                Notifications.Add(new DashboardNotification
                {
                    Icon = "\uf7d9",
                    TypeLabel = "Servisna usluga",
                    DateText = $"Datum usluge: {job.ScheduledDate:dd.MM.yyyy.}",
                    Description = $"Recenzija za uslugu \"{job.Listing.ServiceName}\" servisera '{providerName}' " +
                                  $"može se izmeniti još {FormatEditDeadline(job.Review!.CreatedAt)}. \n" +
                                  $"Trenutna ocena: kvalitet {job.Review.QualityRating}/5, " +
                                  $"komunikacija {job.Review.CommunicationRating}/5 " +
                                  $"(prosek {((job.Review.QualityRating + job.Review.CommunicationRating) / 2.0):F2}/5).",
                    ActionLabel = "Izmeni recenziju",
                    ActionCommand = new RelayCommand(() => OpenReviewWindow(capturedJob, isEdit: true))
                });
            }
        }

        private static string FormatReviewDeadline(BookedJobDto job)
        {
            DateTime completionTime = job.ScheduledTime.HasValue && job.DurationHours.HasValue
                ? job.ScheduledDate.ToDateTime(job.ScheduledTime.Value).AddHours(job.DurationHours.Value)
                : job.ScheduledDate.ToDateTime(new TimeOnly(23, 59, 59));

            TimeSpan remaining = completionTime.AddDays(10) - DateTime.Now;
            return FormatTimeSpan(remaining);
        }

        private static string FormatEditDeadline(DateTime createdAt)
        {
            TimeSpan remaining = createdAt.AddDays(3) - DateTime.Now;
            return FormatTimeSpan(remaining);
        }

        private static string FormatTimeSpan(TimeSpan remaining)
        {
            if (remaining <= TimeSpan.Zero) return "manje od minut";

            int totalDays = (int)remaining.TotalDays;
            int hours = remaining.Hours;
            int minutes = remaining.Minutes;

            if (totalDays >= 1 && hours > 0)
                return $"{totalDays} {Dan(totalDays)} i {hours} {Sat(hours)}";

            if (totalDays >= 1)
                return $"{totalDays} {Dan(totalDays)}";

            if (hours >= 1 && minutes > 0)
                return $"{hours} {Sat(hours)} i {minutes} min";

            if (hours >= 1)
                return $"{hours} {Sat(hours)}";

            return $"{minutes} min";
        }

        private static string Dan(int n) => n == 1 ? "dan" : n is 2 or 3 or 4 ? "dana" : "dana";
        private static string Sat(int n) => n == 1 ? "sat" : n is 2 or 3 or 4 ? "sata" : "sati";

        private void ExecuteViewRequests()
        {
        }

        private void ExecuteRegisterNeighborhood()
        {
        }

        private void ExecuteGenerateReport()
        {
        }

        private void OpenReviewWindow(BookedJobDto job, bool isEdit)
        {
            if (_coordinator is null) return;

            var owner = GetActiveWindow();
            var window = new ProviderReviewWindow(job, _coordinator) { Owner = owner };

            if (window.ShowDialog() == true)
                LoadNotifications();
        }

        private Window GetActiveWindow() =>
            System.Windows.Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)
            ?? System.Windows.Application.Current.MainWindow;

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