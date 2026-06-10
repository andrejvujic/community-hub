using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using CommunityHub.Application.Domain.TrustRecords;
using CommunityHub.Application.Services.Dtos.ServiceBookings;
using CommunityHub.Application.Services.Dtos.Users;
using CommunityHub.Application.Services.JobListingBookings;
using CommunityHub.Application.Services.Neighborhoods;
using CommunityHub.Application.Services.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.Services.Neighborhoods.NeighborhoodMeetings;
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
        private readonly NeighborhoodMeetingService _neighborhoodMeetingService = new();
        private readonly NeighborhoodMembershipService _neighborhoodMembershipService = new();
        private readonly NeighborhoodService _neighborhoodService = new();

        private long _coordinatorId;
        private UserDto? _coordinator;

        public ObservableCollection<DashboardNotification> Notifications { get; } = new();

        public RelayCommand ViewRequestsCommand { get; }
        public RelayCommand RegisterNeighborhoodCommand { get; }
        public RelayCommand GenerateReportCommand { get; }
        private Action<long, long>? _navigateToBudgetAction;

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

        public void Initialize(long coordinatorId, Action<long, long> navigateToBudgetAction)
        {
            _coordinatorId = coordinatorId;
            _navigateToBudgetAction = navigateToBudgetAction;
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
            AddTiedMeetingNotifications();
            AddScheduledMeetingNotifications();
            AddMeetingRecommendationNotifications();
        }

        private void AddDonationNotifications()
        {
            var donations = _budgetDonationService.GetRecentByCoordinator(_coordinatorId);

            foreach (var donation in donations)
            {
                var capturedDonation = donation;

                Notifications.Add(new DashboardNotification
                {
                    Icon = "\uf0d6",
                    TypeLabel = "Donacija građana",
                    DateText = $"Datum: {donation.DonationDate:dd.MM.yyyy.}",
                    Description = $"Izvršena je uplata u iznosu od {donation.Amount:N0} RSD " +
                                  $"za budžet kvarta {donation.Neighborhood.Name} " +
                                  $"(kategorija: {donation.BudgetCategory.Name}).",
                    ActionLabel = "Detalji budžeta",
                    ActionCommand = new RelayCommand(() =>
                        _navigateToBudgetAction?.Invoke(capturedDonation.Neighborhood.Id, capturedDonation.BudgetCategory.Id))
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

        private void AddScheduledMeetingNotifications()
        {
            var scheduled = _neighborhoodMeetingService.GetRecentlyScheduledByCoordinator(_coordinatorId);

            foreach (var meeting in scheduled)
            {
                Notifications.Add(new DashboardNotification
                {
                    Icon = "\uf274",
                    TypeLabel = "Sastanak zakazan",
                    DateText = $"Datum: {meeting.ScheduledDate:dd.MM.yyyy.}",
                    Description = $"Glasanje za sastanak teme \"{meeting.Theme}\" u kvartu {meeting.Neighborhood.Name} " +
                                  $"je završeno. Konačan termin: {meeting.ScheduledDate:dd.MM.yyyy.} u {meeting.MeetingTime:HH:mm}.",
                    ActionLabel = "Prikaži sastanke",
                    //ActionCommand 
                });
            }
        }

        private void AddTiedMeetingNotifications()
        {
            var tiedMeetings = _neighborhoodMeetingService.GetTiedMeetingsByCoordinator(_coordinatorId);

            foreach (var meeting in tiedMeetings)
            {
                var deadline = meeting.DateRangeStart.ToDateTime(meeting.MeetingTime).AddHours(-3);
                var remaining = deadline - DateTime.Now;
                var remainingText = FormatTimeSpan(remaining);
                var capturedMeeting = meeting;

                var tiedDates = _neighborhoodMeetingService.GetTiedDatesForMeeting(meeting.Id);
                var datesText = string.Join(", ", tiedDates.Select(d => d.ToString("dd.MM.yyyy.")));

                Notifications.Add(new DashboardNotification
                {
                    Icon = "\uf017",
                    TypeLabel = "Izjednačeno glasanje",
                    DateText = $"Rok za izbor: {deadline:dd.MM.yyyy. HH:mm}",
                    Description = $"Glasanje za sastanak teme \"{meeting.Theme}\" u kvartu {meeting.Neighborhood.Name} " +
                                  $"je izjednačeno. Preostalo vreme za izbor termina: {remainingText}.\n" +
                                  $"Izjednačeni datumi: {datesText}.",
                    ActionLabel = "Izaberi termin",
                    //ActionCommand
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

        private void AddMeetingRecommendationNotifications()
        {
            var neighborhoods = _neighborhoodService.GetByCoordinator(_coordinatorId);

            foreach (var neighborhood in neighborhoods)
            {
                var records = _neighborhoodMembershipService.GetTrustRecordsByNeighborhood(neighborhood.Id);
                if (records.Count == 0) continue;

                int newCount = records.Count(r => r.TrustStatus == TrustStatus.New);
                int inactiveCount = records.Count(r => r.TrustStatus == TrustStatus.Inactive);
                int total = records.Count;

                int maxCount = new[]
                {
                    newCount, inactiveCount,
                    records.Count(r => r.TrustStatus == TrustStatus.Active),
                    records.Count(r => r.TrustStatus == TrustStatus.Prominent),
                    records.Count(r => r.TrustStatus == TrustStatus.Trustworthy)
                }.Max();

                if (maxCount == newCount && newCount > 0)
                {
                    double pct = Math.Round((double)newCount / total * 100, 2);
                    Notifications.Add(new DashboardNotification
                    {
                        Icon = "\uf0eb",
                        TypeLabel = "Preporuka sistema",
                        DateText = $"Kvart: {neighborhood.Name}",
                        Description = $"U kvartu {neighborhood.Name} je najveći broj novih građana " +
                                      $"({newCount} od {total}, {pct}%). " +
                                      $"Predlog: Organizujte sastanak dobrodošlice.",
                        ActionLabel = "Organizuj",
                        //ActionCommand
                    });
                }
                else if (maxCount == inactiveCount && inactiveCount > 0)
                {
                    double pct = Math.Round((double)inactiveCount / total * 100, 2);
                    Notifications.Add(new DashboardNotification
                    {
                        Icon = "\uf0eb",
                        TypeLabel = "Preporuka sistema",
                        DateText = $"Kvart: {neighborhood.Name}",
                        Description = $"U kvartu {neighborhood.Name} je najveći broj neaktivnih građana " +
                                      $"({inactiveCount} od {total}, {pct}%). " +
                                      $"Predlog: Organizujte sastanak motivacije.",
                        ActionLabel = "Organizuj",
                        //ActionCommand
                    });
                }
            }
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