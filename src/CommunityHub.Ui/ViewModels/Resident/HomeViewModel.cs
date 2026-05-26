using CommunityHub.Application;
using CommunityHub.Application.Domain.Notifications;
using CommunityHub.Application.Domain.Notifications.RepositoryInterfaces;
using CommunityHub.Application.Domain.RepositoryInterfaces.Ads;
using CommunityHub.Application.Modules.Buildings.Core.Services.Ads;
using CommunityHub.Application.Services.Notifications;
using CommunityHub.Application.Services.ResidentAssemblies;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Notifications;

namespace CommunityHub.Ui.ViewModels.Resident
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly NotificationService _notificationService;
        private readonly AdService _adService;
        private readonly AssemblyService _assemblyService;

        public HomeViewModel(
            long userId,
            IMainNavigationConsumer navigationConsumer
        ) : base(navigationConsumer) {
            UserId = userId;

            _notificationService = new(
                Injector.CreateInstance<INotificationRepository>()
            );

            _adService = new(
                Injector.CreateInstance<IAdRepository>(),
                Injector.CreateInstance<IAdTimeSlotRepository>()
            );

            _assemblyService = new();

            OpenNotificationsCommand = new(OpenNotificationsPage);

            Initialize();
        }

        public long UserId { get; }
        public RelayCommand OpenNotificationsCommand { get; }

        private void OpenNotificationsPage()
            => Push(
                new NotificationsPage(UserId),
                onPop: _ => UpdateUnreadNotificationCount()
            );

        private void Initialize()
        {
            UpdateUnreadNotificationCount();
            _adService.ArchiveExpiredAds();
            _assemblyService.RefreshStatuses();
        }

        private void UpdateUnreadNotificationCount()
        {
            UnreadNotificationCount = _notificationService.GetNotificationCountForUser(
                UserId,
                status: NotificationStatus.Unread
            );
        }

        private int _unreadNotificationCount = 0;
        public int UnreadNotificationCount
        {
            get => _unreadNotificationCount;
            private set
            {
                if (_unreadNotificationCount == value) return;
                _unreadNotificationCount = value;
                OnPropertyChanged(nameof(UnreadNotificationCount));
                OnPropertyChanged(nameof(UnreadNotificationCountText));
                OnPropertyChanged(nameof(HasUnreadNotifications));
            }
        }

        public string UnreadNotificationCountText
        {
            get
            {
                const int thresholdNotificationCount = 9;
                if (UnreadNotificationCount > thresholdNotificationCount) return $"{thresholdNotificationCount}+";
                return UnreadNotificationCount.ToString();
            }
        }

        public bool HasUnreadNotifications => UnreadNotificationCount > 0;

    }
}
