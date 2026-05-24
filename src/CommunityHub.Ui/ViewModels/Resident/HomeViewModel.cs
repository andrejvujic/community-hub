using CommunityHub.Application;
using CommunityHub.Application.Domain.Notifications;
using CommunityHub.Application.Domain.Notifications.RepositoryInterfaces;
using CommunityHub.Application.Domain.RepositoryInterfaces.Ads;
using CommunityHub.Application.Modules.Buildings.Core.Services.Ads;
using CommunityHub.Application.Services.Notifications;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Apartments;
using CommunityHub.Ui.Views.Resident.Buildings.Browse;
using CommunityHub.Ui.Views.Resident.Notifications;

namespace CommunityHub.Ui.ViewModels.Resident
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly NotificationService _notificationService;
        private readonly AdService _adService;
        private readonly long _userId;
        private readonly Action<MainNavigationConsumerPage> _navigateLocal;

        public HomeViewModel(
            long userId,
            IMainNavigationConsumer navigationConsumer,
            Action<MainNavigationConsumerPage> navigateLocal
        ) : base(navigationConsumer) {
            _notificationService = new(
                Injector.CreateInstance<INotificationRepository>()
            );

            _adService = new(
                Injector.CreateInstance<IAdRepository>(),
                Injector.CreateInstance<IAdTimeSlotRepository>()
            );

            _userId = userId;
            _navigateLocal = navigateLocal;

            OpenNotificationsCommand = new(OpenNotificationsPage);
            OpenBuildingsCommand = new(OpenBuildingsPage);
            OpenApartmentAccessRequestsCommand = new(OpenApartmentAccessRequestsPage);
        }

        public RelayCommand OpenNotificationsCommand { get; }
        public RelayCommand OpenBuildingsCommand { get; }
        public RelayCommand OpenApartmentAccessRequestsCommand { get; }

        private void OpenNotificationsPage()
            => Push(
                new NotificationsPage(_userId),
                onPop: _ => UpdateUnreadNotificationCount()
            );

        private void OpenBuildingsPage()
            => _navigateLocal(new BuildingsPage(_userId));

        private void OpenApartmentAccessRequestsPage()
            => _navigateLocal(new ApartmentAccessRequestsPage(_userId));

        public void Initialize()
        {
            UpdateUnreadNotificationCount();
            _adService.ArchiveExpiredAds();
            OpenBuildingsPage();
        }

        private void UpdateUnreadNotificationCount()
        {
            UnreadNotificationCount = _notificationService.GetNotificationCountForUser(
                _userId,
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
