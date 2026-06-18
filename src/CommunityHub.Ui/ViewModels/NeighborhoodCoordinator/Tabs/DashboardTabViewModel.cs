using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
using CommunityHub.Application.Services.Neighborhoods.CityFacilities.Reports;
using CommunityHub.Application.Services.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.Services.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.Services.Users;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using CommunityHub.Ui.Resources.Localization;

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
        public ObservableCollection<NeighborhoodDto> Neighborhoods { get; } = new();

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
        
        public string PendingRequestsText => TranslationSource.Instance.Format("PendingRequests_Text", PendingRequestsCount);


        private DateTime? _reportFrom;
        public DateTime? ReportFrom
        {
            get => _reportFrom;
            set
            {
                _reportFrom = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _reportTo;
        public DateTime? ReportTo
        {
            get => _reportTo;
            set
            {
                _reportTo = value;
                OnPropertyChanged();
            }
        }

        private NeighborhoodDto? _selectedReportNeighborhood;
        public NeighborhoodDto? SelectedReportNeighborhood
        {
            get => _selectedReportNeighborhood;
            set 
            { 
                _selectedReportNeighborhood = value; 
                OnPropertyChanged(); 
            }
        }

        public DashboardTabViewModel()
        {
            ViewRequestsCommand = new RelayCommand(ExecuteViewRequests);
            RegisterNeighborhoodCommand = new RelayCommand(ExecuteRegisterNeighborhood);
            GenerateReportCommand = new RelayCommand(ExecuteGenerateReport);

            TranslationSource.Instance.PropertyChanged += (s, e) => LoadDashboard();
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
            var window = GetActiveWindow();

            if (!TryValidateReportInputs(window, out DateTime from, out DateTime to))
                return;

            var savePath = ChooseSavePath(from, ReportTo!.Value, SelectedReportNeighborhood!.Name);
            if (savePath is null) return;

            try
            {
                QuestPDF.Settings.License = LicenseType.Community;

                var reportService = new FacilityReservationsReportService();
                var pdfBytes = reportService.GenerateBytes(
                    SelectedReportNeighborhood!.Name,
                    SelectedReportNeighborhood.Id,
                    from,
                    to);

                File.WriteAllBytes(savePath, pdfBytes);
                AskToOpenReport(window, savePath);
            }
            catch (Exception ex)
            {
                new InfoDialog(TranslationSource.Instance["Error_Title"], TranslationSource.Instance.Format("Error_Generate", ex.Message), window).ShowDialog();
            }
        }

        private bool TryValidateReportInputs(Window window, out DateTime from, out DateTime to)
        {
            from = DateTime.MinValue;
            to = DateTime.MinValue;

            if (SelectedReportNeighborhood is null)
            {
                new InfoDialog(TranslationSource.Instance["Error_Title"], TranslationSource.Instance["Error_SelectNeighborhood"], window).ShowDialog();
                
                return false;
            }

            if (ReportFrom is null || ReportTo is null)
            {
                new InfoDialog(TranslationSource.Instance["Error_Title"], TranslationSource.Instance["Error_SelectDates"], window).ShowDialog();
                return false;
            }

            if (ReportFrom > ReportTo)
            {
                new InfoDialog(TranslationSource.Instance["Error_Title"], TranslationSource.Instance["Error_DateOrder"], window).ShowDialog();
                return false;
            }

            from = ReportFrom.Value;
            to = ReportTo.Value.AddDays(1).AddSeconds(-1);
            return true;
        }

        private static string? ChooseSavePath(DateTime from, DateTime to, string neighborhoodName)
        {
            var fromStr = from.ToString("yyyy-MM-dd");
            var toStr = to.ToString("yyyy-MM-dd");

            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Title = TranslationSource.Instance["Report_SaveTitle"],
                Filter = TranslationSource.Instance["Report_SaveFilter"],
                FileName = TranslationSource.Instance.Format("Report_FileName", neighborhoodName, fromStr, toStr),
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads"
            };

            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }

        private void AskToOpenReport(Window window, string savePath)
        {
            var confirm = new ConfirmDialog(
                TranslationSource.Instance["Report_Success_Title"],
                TranslationSource.Instance["Report_Success_Message"],
                window);

            if (confirm.ShowDialog() == true)
                OpenPdf(savePath);
        }

        private static void OpenPdf(string filePath)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(filePath)
            {
                UseShellExecute = true
            });
        }

        public void RefreshFilters() => ApplyFilter();

        public void Initialize(long coordinatorId, Action<long, long> navigateToBudgetAction, Action<long> navigateToMeetingsAction, Action navigateToRequests)
        {
            _coordinatorId = coordinatorId;
            _navigateToBudgetAction = navigateToBudgetAction;
            _navigateToMeetingsAction = navigateToMeetingsAction;
            _navigateToRequestsAction = navigateToRequests;
            _coordinator = _userService.Get(_coordinatorId);
            ReportFrom = DateTime.Today.AddMonths(-1);
            ReportTo = DateTime.Today;

            LoadDashboard();
        }


        public void LoadDashboard()
        {
            LoadNeighborhoodsForReports();
            LoadPendingRequests();
            LoadNotifications();
            LoadNeighborhoodsForReports();
        }

        private void LoadNeighborhoodsForReports()
        {
            Neighborhoods.Clear();
            var list = _neighborhoodService.GetByCoordinator(_coordinatorId);
            foreach (var n in list) Neighborhoods.Add(n);
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
                list.Add(CreateRecommendationNotification(neighborhood, "", MeetingTheme.Welcome, newCount, total));
            else if (maxCount == inactiveCount && inactiveCount > 0)
                list.Add(CreateRecommendationNotification(neighborhood, "", MeetingTheme.Motivation, inactiveCount, total));
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
                .Select(d => TranslationSource.Instance.FormatDate(d)));

            return new DashboardNotification
            {
                Icon = "\uf017",
                TypeLabel = TranslationSource.Instance["Notification_TiedVoting_Type"],
                DateText = TranslationSource.Instance.Format("Notification_TiedVoting_Deadline", TranslationSource.Instance.FormatDateTime(deadline)),
                Description = TranslationSource.Instance.Format("Notification_TiedVoting_Description", ThemeDisplay(meeting.Theme), meeting.Neighborhood.Name, remainingText, datesText),
                ActionLabel = TranslationSource.Instance["Notification_TiedVoting_Action"],
                ActionCommand = new RelayCommand(() => _navigateToMeetingsAction?.Invoke(meeting.Neighborhood.Id)),
                Category = DashboardNotification.NotificationCategory.Meetings,
            };
        }

        private DashboardNotification CreateRecommendationNotification(NeighborhoodDto neighborhood, string descriptionSuffix, MeetingTheme theme, int count, int total)
        {
            double pct = Math.Round((double)count / total * 100, 2);
            string desc = theme == MeetingTheme.Welcome
                ? TranslationSource.Instance.Format("Notification_Recommendation_Welcome", neighborhood.Name, count, total, pct)
                : theme == MeetingTheme.Motivation
                    ? TranslationSource.Instance.Format("Notification_Recommendation_Motivation", neighborhood.Name, count, total, pct)
                    : $"{descriptionSuffix} ({count} od {total}, {pct}%).";

            return new DashboardNotification
            {
                Icon = "\uf0eb",
                TypeLabel = TranslationSource.Instance["Notification_Recommendation_Type"],
                DateText = TranslationSource.Instance.Format("Dashboard_Neighborhood", neighborhood.Name),
                Description = desc,
                ActionLabel = TranslationSource.Instance["Notification_Recommendation_Action"],
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
                TypeLabel = TranslationSource.Instance["Notification_Scheduled_Type"],
                DateText = TranslationSource.Instance.Format("Notification_Scheduled_Date", TranslationSource.Instance.FormatDate(meeting.ScheduledDate)),
                Description = TranslationSource.Instance.Format("Notification_Scheduled_Description", ThemeDisplay(meeting.Theme), meeting.Neighborhood.Name, TranslationSource.Instance.FormatDate(meeting.ScheduledDate), TranslationSource.Instance.FormatTime(meeting.MeetingTime)),
                ActionLabel = TranslationSource.Instance["Notification_Scheduled_Action"],
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
                TypeLabel = TranslationSource.Instance["Notification_Service_Type"],
                DateText = TranslationSource.Instance.Format("Notification_Service_Date", TranslationSource.Instance.FormatDate(job.ScheduledDate.ToDateTime(TimeOnly.MinValue))),
                Description = TranslationSource.Instance.Format("Notification_Service_Description", providerName, job.Listing.ServiceName, $"{job.TotalPrice:N2}", FormatTimeSpan(deadline - DateTime.Now)),
                ActionLabel = TranslationSource.Instance["Notification_Service_Action"],
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
                TypeLabel = TranslationSource.Instance["Notification_EditReview_Type"],
                DateText = TranslationSource.Instance.Format("Notification_EditReview_Date", TranslationSource.Instance.FormatDate(job.ScheduledDate.ToDateTime(TimeOnly.MinValue))),
                Description = TranslationSource.Instance.Format("Notification_EditReview_Description", job.Listing.ServiceName, providerName, FormatTimeSpan(deadline - DateTime.Now), job.Review.QualityRating, job.Review.CommunicationRating, $"{((job.Review.QualityRating + job.Review.CommunicationRating) / 2.0):F2}"),
                ActionLabel = TranslationSource.Instance["Notification_EditReview_Action"],
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
                TypeLabel = TranslationSource.Instance["Notification_Donation_Type"],
                DateText = TranslationSource.Instance.Format("Notification_Donation_Date", TranslationSource.Instance.FormatDate(donation.DonationDate)),
                Description = TranslationSource.Instance.Format("Notification_Donation_Description", $"{donation.Amount:N2}", donation.Neighborhood.Name, donation.BudgetCategory.Name),
                ActionLabel = TranslationSource.Instance["Notification_Donation_Action"],
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

            var recommendations = filtered.Where(x => x.TypeLabel == TranslationSource.Instance["Notification_Recommendation_Type"]).ToList();
            var meetings = filtered.Where(x => x.Category == DashboardNotification.NotificationCategory.Meetings
                                            && x.TypeLabel != TranslationSource.Instance["Notification_Recommendation_Type"]).ToList();
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
            bool isEnglish = TranslationSource.Instance.CurrentCulture?.Name == "en-US";
            if (remaining <= TimeSpan.Zero) return isEnglish ? "less than a minute" : "manje od minut";

            int totalDays = (int)remaining.TotalDays;
            int hours = remaining.Hours;
            int minutes = remaining.Minutes;

            string andWord = isEnglish ? "and" : "i";
            string minWord = "min";

            if (totalDays >= 1 && hours > 0)
                return $"{totalDays} {Dan(totalDays)} {andWord} {hours} {Sat(hours)}";

            if (totalDays >= 1)
                return $"{totalDays} {Dan(totalDays)}";

            if (hours >= 1 && minutes > 0)
                return $"{hours} {Sat(hours)} {andWord} {minutes} {minWord}";

            if (hours >= 1)
                return $"{hours} {Sat(hours)}";

            return $"{minutes} {minWord}";
        }

        private static string Dan(int n)
        {
            bool isEnglish = TranslationSource.Instance.CurrentCulture?.Name == "en-US";
            if (isEnglish) return n == 1 ? "day" : "days";

            int lastDigit = n % 10;
            int lastTwo = n % 100;
            if (lastDigit == 1 && lastTwo != 11) return "dan";
            return "dana";
        }

        private static string Sat(int n)
        {
            bool isEnglish = TranslationSource.Instance.CurrentCulture?.Name == "en-US";
            if (isEnglish) return n == 1 ? "hour" : "hours";

            int lastDigit = n % 10;
            int lastTwo = n % 100;
            if (lastDigit == 1 && lastTwo != 11) return "sat";
            if (lastDigit >= 2 && lastDigit <= 4 && (lastTwo < 10 || lastTwo > 20)) return "sata";
            return "sati";
        }

        private static string ThemeDisplay(MeetingTheme theme) => theme switch
        {
            MeetingTheme.Welcome => TranslationSource.Instance["MeetingTheme_Welcome"],
            MeetingTheme.Motivation => TranslationSource.Instance["MeetingTheme_Motivation"],
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
                new InfoDialog(TranslationSource.Instance["Success_Title"], isEdit ? TranslationSource.Instance["Review_EditSuccess"] : TranslationSource.Instance["Review_RateSuccess"], owner).ShowDialog();
                LoadNotifications();
            }
        }

        private void OpenScheduleMeetingWindow(NeighborhoodDto neighborhood, MeetingTheme theme)
        {
            var owner = GetActiveWindow();
            var window = new ScheduleMeetingWindow(neighborhood, theme) { Owner = owner };
            if (window.ShowDialog() == true)
            {
                new InfoDialog(TranslationSource.Instance["Success_Title"], TranslationSource.Instance["Meeting_ScheduleSuccess"], owner).ShowDialog();
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