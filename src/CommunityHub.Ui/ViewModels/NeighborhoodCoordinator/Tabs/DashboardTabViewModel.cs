using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.Domain.TrustRecords;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodMeetings;
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
        private readonly NeighborhoodAccessRequestService _requestService = new();

        private long _coordinatorId;
        private UserDto? _coordinator;

        private Action<long, long>? _navigateToBudgetAction;
        private Action<long>? _navigateToMeetingsAction;
        private Action? _navigateToRequestsAction;

        public ObservableCollection<DashboardNotification> Notifications { get; } = new();

        public RelayCommand ViewRequestsCommand { get; }
        public RelayCommand RegisterNeighborhoodCommand { get; }
        public RelayCommand GenerateReportCommand { get; }
        private bool _showServiceFilter = true;
        public bool ShowServiceFilter
        {
            get => _showServiceFilter;
            set 
            { 
                _showServiceFilter = value; 
                OnPropertyChanged(); 
                ApplyFilter(); 
            }
        }

        private bool _showMeetingsFilter = true;
        public bool ShowMeetingsFilter
        {
            get => _showMeetingsFilter;
            set 
            { 
                _showMeetingsFilter = value; 
                OnPropertyChanged(); 
                ApplyFilter(); 
            }
        }

        private bool _showDonationsFilter = true;
        public bool ShowDonationsFilter
        {
            get => _showDonationsFilter;
            set 
            { 
                _showDonationsFilter = value; 
                OnPropertyChanged(); 
                ApplyFilter(); 
            }
        }

        private List<DashboardNotification> _allNotifications = new();
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

        public DateTime? ReportFrom { get; set; }
        public DateTime? ReportTo { get; set; }

        public DashboardTabViewModel()
        {
            ViewRequestsCommand = new RelayCommand(ExecuteViewRequests);
            RegisterNeighborhoodCommand = new RelayCommand(ExecuteRegisterNeighborhood);
            GenerateReportCommand = new RelayCommand(ExecuteGenerateReport);
        }

        private void ExecuteViewRequests()
        {
            _navigateToRequestsAction?.Invoke();
        }

        private void ExecuteRegisterNeighborhood()
        {
            var owner = GetActiveWindow();
            var window = new RegisterNeighborhoodWindow(_coordinatorId) { Owner = owner };
            if (window.ShowDialog() == true)
            {
                LoadNotifications();
            }
        }

        private void ExecuteGenerateReport()
        {
            var owner = GetActiveWindow();
            new InfoDialog("Info", "Generisanje izveštaja nije implementirano.", owner).ShowDialog();
        }

        public void RefreshFilters() => ApplyFilter();

        public void Initialize(long coordinatorId, Action<long, long> navigateToBudgetAction, Action<long> navigateToMeetingsAction, Action navigateToRequests)
        {
            _coordinatorId = coordinatorId;
            _navigateToBudgetAction = navigateToBudgetAction;
            _navigateToMeetingsAction = navigateToMeetingsAction;
            _navigateToRequestsAction = navigateToRequests;
            _coordinator = _userService.Get(_coordinatorId);

            LoadDashboard();
        }


        public void LoadDashboard()
        {
            LoadPendingRequests();
            LoadNotifications();
        }

        private void LoadPendingRequests()
        {
            (_, PendingRequestsCount, _) = _requestService.GetRequestStatsByCoordinator(_coordinatorId);
        }

        private void LoadNotifications()
        {
            _allNotifications = new List<DashboardNotification>();
            AddMeetingRecommendationNotifications(_allNotifications);
            AddTiedMeetingNotifications(_allNotifications);
            AddScheduledMeetingNotifications(_allNotifications);
            AddServiceNotifications(_allNotifications);
            AddDonationNotifications(_allNotifications);
            ApplyFilter();
        }

        private void AddMeetingRecommendationNotifications(List<DashboardNotification> list)
        {
            var neighborhoods = _neighborhoodService.GetByCoordinator(_coordinatorId);
            foreach (var neighborhood in neighborhoods)
                AddRecommendationForNeighborhood(list, neighborhood);
        }

        private void AddRecommendationForNeighborhood(List<DashboardNotification> list, NeighborhoodDto neighborhood)
        {
            var records = _neighborhoodMembershipService.GetTrustRecordsByNeighborhood(neighborhood.Id);
            if (records.Count == 0) return;

            int newCount = records.Count(r => r.TrustStatus == TrustStatus.New);
            int inactiveCount = records.Count(r => r.TrustStatus == TrustStatus.Inactive);
            int total = records.Count;
            int maxCount = new[] { newCount, inactiveCount, records.Count(r => r.TrustStatus == TrustStatus.Active), records.Count(r => r.TrustStatus == TrustStatus.Prominent), records.Count(r => r.TrustStatus == TrustStatus.Trustworthy) }.Max();

            if (maxCount == newCount && newCount > 0)
                list.Add(CreateRecommendationNotification(neighborhood, "Organizujte sastanak dobrodošlice", MeetingTheme.Welcome, newCount, total));
            else if (maxCount == inactiveCount && inactiveCount > 0)
                list.Add(CreateRecommendationNotification(neighborhood, "Organizujte sastanak motivacije", MeetingTheme.Motivation, inactiveCount, total));
        }

        private void AddTiedMeetingNotifications(List<DashboardNotification> list)
        {
            var tiedMeetings = _neighborhoodMeetingService.GetTiedMeetingsByCoordinator(_coordinatorId);
            foreach (var meeting in tiedMeetings)
                list.Add(BuildTiedMeetingNotification(meeting));
        }

        private DashboardNotification BuildTiedMeetingNotification(NeighborhoodMeetingDto meeting)
        {
            var deadline = meeting.DateRangeStart.ToDateTime(meeting.MeetingTime).AddHours(-3);
            var remainingText = FormatTimeSpan(deadline - DateTime.Now);
            var datesText = string.Join(", ", _neighborhoodMeetingService.GetTiedDatesForMeeting(meeting.Id)
                .Select(d => d.ToString("dd.MM.yyyy.")));

            return new DashboardNotification
            {
                Icon = "\uf017",
                TypeLabel = "Izjednačeno glasanje",
                DateText = $"Rok za izbor: {deadline:dd.MM.yyyy. HH:mm}",
                Description = $"Glasanje za sastanak teme \"{ThemeDisplay(meeting.Theme)}\" u kvartu {meeting.Neighborhood.Name} " +
                              $"je izjednačeno. Preostalo vreme za izbor termina: {remainingText}.\n" +
                              $"Izjednačeni datumi: {datesText}.",
                ActionLabel = "Izaberi termin",
                ActionCommand = new RelayCommand(() => _navigateToMeetingsAction?.Invoke(meeting.Neighborhood.Id)),
                Category = DashboardNotification.NotificationCategory.Meetings,
            };
        }

        private DashboardNotification CreateRecommendationNotification(NeighborhoodDto neighborhood, string descriptionSuffix, MeetingTheme theme, int count, int total)
        {
            double pct = Math.Round((double)count / total * 100, 2);
            string desc = theme == MeetingTheme.Welcome
                ? $"U kvartu {neighborhood.Name} je najveći broj novih građana ({count} od {total}, {pct}%).\nPredlog: Organizujte sastanak dobrodošlice."
                : theme == MeetingTheme.Motivation
                    ? $"U kvartu {neighborhood.Name} je najveći broj neaktivnih građana ({count} od {total}, {pct}%).\nPredlog: Organizujte sastanak motivacije."
                    : $"{descriptionSuffix} ({count} od {total}, {pct}%).";

            return new DashboardNotification
            {
                Icon = "\uf0eb",
                TypeLabel = "Preporuka sistema",
                DateText = $"Kvart: {neighborhood.Name}",
                Description = desc,
                ActionLabel = "Organizuj",
                ActionCommand = new RelayCommand(() => OpenScheduleMeetingWindow(neighborhood, theme)),
                Category = DashboardNotification.NotificationCategory.Meetings,
            };
        }

        private void AddScheduledMeetingNotifications(List<DashboardNotification> list)
        {
            var scheduled = _neighborhoodMeetingService.GetRecentlyScheduledByCoordinator(_coordinatorId);
            foreach (var meeting in scheduled)
                list.Add(CreateScheduledMeetingNotification(meeting));
        }

        private DashboardNotification CreateScheduledMeetingNotification(NeighborhoodMeetingDto meeting)
        {
            return new DashboardNotification
            {
                Icon = "\uf274",
                TypeLabel = "Sastanak zakazan",
                DateText = $"Datum: {meeting.ScheduledDate:dd.MM.yyyy.}",
                Description = $"Glasanje za sastanak teme \"{ThemeDisplay(meeting.Theme)}\" u kvartu {meeting.Neighborhood.Name} " +
                              $"je završeno.\n" +
                              $"Konačan termin: {meeting.ScheduledDate:dd.MM.yyyy.} u {meeting.MeetingTime:HH:mm}.",
                ActionLabel = "Prikaži sastanke",
                ActionCommand = new RelayCommand(() => _navigateToMeetingsAction?.Invoke(meeting.Neighborhood.Id)),
                Category = DashboardNotification.NotificationCategory.Meetings,
            };
        }

        private void AddServiceNotifications(List<DashboardNotification> list)
        {
            AddUnreviewedServiceNotifications(list);
            AddEditableReviewNotifications(list);
        }

        private void AddUnreviewedServiceNotifications(List<DashboardNotification> list)
        {
            if (_coordinator is null) return;
            var unreviewed = _bookedJobService.GetRecentUnreviewedByCoordinator(_coordinatorId);
            foreach (var job in unreviewed)
                list.Add(CreateUnreviewedNotification(job));
        }

        private DashboardNotification CreateUnreviewedNotification(BookedJobDto job)
        {
            var providerName = $"{job.Listing.Provider.Name} {job.Listing.Provider.Surname}".Trim();
            var deadline = ReviewDeadline(job);
            return new DashboardNotification
            {
                Icon = "\uf7d9",
                TypeLabel = "Servisna usluga",
                DateText = $"Datum: {job.ScheduledDate:dd.MM.yyyy.}",
                Description = $"Serviser '{providerName}' je obavio uslugu \"{job.Listing.ServiceName}\". Trošak {job.TotalPrice:N2} RSD. \nOceniti možete još {FormatTimeSpan(deadline - DateTime.Now)}.",
                ActionLabel = "Oceni",
                ActionCommand = new RelayCommand(() => OpenReviewWindow(job, isEdit: false)),
                Category = DashboardNotification.NotificationCategory.Service,
            };
        }

        private void AddEditableReviewNotifications(List<DashboardNotification> list)
        {
            if (_coordinator is null) return;
            var editable = _bookedJobService.GetEditableByCoordinator(_coordinatorId);
            foreach (var job in editable)
                list.Add(CreateEditableReviewNotification(job));
        }

        private DashboardNotification CreateEditableReviewNotification(BookedJobDto job)
        {
            var providerName = $"{job.Listing.Provider.Name} {job.Listing.Provider.Surname}".Trim();
            var deadline = job.Review!.CreatedAt.AddDays(3);
            return new DashboardNotification
            {
                Icon = "\uf7d9",
                TypeLabel = "Izmena recenzije",
                DateText = $"Datum usluge: {job.ScheduledDate:dd.MM.yyyy.}",
                Description = $"Recenzija za uslugu \"{job.Listing.ServiceName}\" servisera '{providerName}' " +
                                  $"može se izmeniti još {FormatTimeSpan(deadline - DateTime.Now)}. \n" +
                                  $"Trenutna ocena: kvalitet {job.Review.QualityRating}/5, " +
                                  $"komunikacija {job.Review.CommunicationRating}/5 " +
                                  $"(prosek {((job.Review.QualityRating + job.Review.CommunicationRating) / 2.0):F2}/5).",
                ActionLabel = "Izmeni recenziju",
                ActionCommand = new RelayCommand(() => OpenReviewWindow(job, isEdit: true)),
                Category = DashboardNotification.NotificationCategory.Service,
            };
        }

        private void AddDonationNotifications(List<DashboardNotification> list)
        {
            var donations = _budgetDonationService.GetRecentByCoordinator(_coordinatorId);
            foreach (var donation in donations)
                list.Add(CreateDonationNotification(donation));
        }

        private DashboardNotification CreateDonationNotification(BudgetDonationDto donation)
        {
            return new DashboardNotification
            {
                Icon = "\uf0d6",
                TypeLabel = "Donacija građana",
                DateText = $"Datum: {donation.DonationDate:dd.MM.yyyy.}",
                Description = $"Izvršena je uplata u iznosu od {donation.Amount:N2} RSD " +
                              $"za budžet kvarta {donation.Neighborhood.Name} " +
                              $"(kategorija: {donation.BudgetCategory.Name}).",
                ActionLabel = "Detalji budžeta",
                ActionCommand = new RelayCommand(() => _navigateToBudgetAction?.Invoke(donation.Neighborhood.Id, donation.BudgetCategory.Id)),
                Category = DashboardNotification.NotificationCategory.Donations,
            };
        }

        private void ApplyFilter()
        {
            Notifications.Clear();
            if (_allNotifications == null || _allNotifications.Count == 0) return;

            var filtered = _allNotifications
                .Where(x =>
                    (x.Category == DashboardNotification.NotificationCategory.Service && ShowServiceFilter) ||
                    (x.Category == DashboardNotification.NotificationCategory.Meetings && ShowMeetingsFilter) ||
                    (x.Category == DashboardNotification.NotificationCategory.Donations && ShowDonationsFilter))
                .ToList();

            var recommendations = filtered.Where(x => x.TypeLabel == "Preporuka sistema").ToList();
            var meetings = filtered.Where(x => x.Category == DashboardNotification.NotificationCategory.Meetings && x.TypeLabel != "Preporuka sistema").ToList();
            var services = filtered.Where(x => x.Category == DashboardNotification.NotificationCategory.Service).ToList();
            var donations = filtered.Where(x => x.Category == DashboardNotification.NotificationCategory.Donations).ToList();

            foreach (var item in recommendations) Notifications.Add(item);
            foreach (var item in meetings) Notifications.Add(item);
            foreach (var item in services) Notifications.Add(item);
            foreach (var item in donations) Notifications.Add(item);
        }

        private static DateTime ReviewDeadline(BookedJobDto job)
        {
            DateTime completionTime = job.ScheduledTime.HasValue && job.DurationHours.HasValue
                ? job.ScheduledDate.ToDateTime(job.ScheduledTime.Value).AddHours(job.DurationHours.Value)
                : job.ScheduledDate.ToDateTime(new TimeOnly(23, 59, 59));
            return completionTime.AddDays(10);
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

        private static string Dan(int n) => n == 1 ? "dan" : "dana";
        private static string Sat(int n) => n == 1 ? "sat" : n is 2 or 3 or 4 ? "sata" : "sati";

        private static string ThemeDisplay(MeetingTheme theme) => theme switch
        {
            MeetingTheme.Welcome => "Dobrodošlica",
            MeetingTheme.Motivation => "Motivacija",
            _ => theme.ToString()
        };

        private Window GetActiveWindow() =>
            System.Windows.Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive) 
            ?? System.Windows.Application.Current.MainWindow;

        private void OpenReviewWindow(BookedJobDto job, bool isEdit)
        {
            if (_coordinator is null) return;
            var owner = GetActiveWindow();
            var window = new ProviderReviewWindow(job, _coordinator) { Owner = owner };
            if (window.ShowDialog() == true)
            {
                new InfoDialog("Uspeh", isEdit ? "Recenzija je uspešno izmenjena." : "Usluga je uspešno ocenjena.", owner).ShowDialog();
                LoadNotifications();
            }
        }

        private void OpenScheduleMeetingWindow(NeighborhoodDto neighborhood, MeetingTheme theme)
        {
            var owner = GetActiveWindow();
            var window = new ScheduleMeetingWindow(neighborhood, theme) { Owner = owner };
            if (window.ShowDialog() == true)
            {
                new InfoDialog("Uspeh", "Glasanje za novi sastanak je pokrenuto.", owner).ShowDialog();
                LoadNotifications();
            }
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
        public NotificationCategory Category { get; init; }

        public enum NotificationCategory { Meetings, Service, Donations }
    }
}