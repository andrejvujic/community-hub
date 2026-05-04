using CommunityHub.Application.Modules.Buildings.Database.Repositories.Ads;
using CommunityHub.Application.Modules.Shared.Core.Domain.Notifications;
using CommunityHub.Application.Shared.Database.Repositories.Notifications;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Apartments;
using CommunityHub.Ui.Views.Resident.Buildings.Browse;
using CommunityHub.Ui.Views.Resident.Notifications;
using System.ComponentModel;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident
{
    public partial class HomePage : MainNavigatorConsumerPage, INotifyPropertyChanged
    {
        private readonly long _userId;

        private readonly NotificationRepository _notificationRepository = new();
        private readonly AdRepository _adRepository = new();
        public HomePage(long userId)
        {
            InitializeComponent();
            DataContext = this;

            _userId = userId;

            UpdateUnreadNotificationCount();
            _adRepository.ArchiveExpiredAds();

            OpenBuildingsPage();
        }

        private void UpdateUnreadNotificationCount()
        {
            UnreadNotificationCount = _notificationRepository.GetCountByUserId(
                _userId,
                NotificationStatus.Unread
            );
        }

        private int _unreadNotificationCount = 0;
        public int UnreadNotificationCount {
            get => _unreadNotificationCount;
            set
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

        private void OpenNotificationsPage() {
            Push(
                new NotificationsPage(_userId),
                onPop: _ => UpdateUnreadNotificationCount()
            );
        }

        private void OpenBuildingsPage() => LocalFrame.Navigate(new BuildingsPage(_userId));
        private void OpenApartmentAccessRequestsPage() => LocalFrame.Navigate(new ApartmentAccessRequestsPage(_userId));
        private void TopBar_NotificationsItemClick(object sender, MouseButtonEventArgs e)
                => OpenNotificationsPage();

        private void BottomBar_RequestsItemClick(object sender, MouseButtonEventArgs e) => OpenApartmentAccessRequestsPage();
        private void BottomBar_BuildingsItemClick(object sender, MouseButtonEventArgs e) => OpenBuildingsPage();

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
