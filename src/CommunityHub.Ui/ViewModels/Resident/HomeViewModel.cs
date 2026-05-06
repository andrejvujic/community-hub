using CommunityHub.Application;
using CommunityHub.Application.Modules.Buildings.API.Public.Ads;
using CommunityHub.Application.Modules.Shared.API.Public.Notifications;
using CommunityHub.Application.Modules.Shared.Core.Domain.Notifications;
using System.ComponentModel;

namespace CommunityHub.Ui.ViewModels.Resident
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private readonly INotificationService _notificationService = Injector.CreateInstance<INotificationService>();
        private readonly IAdService _adService = Injector.CreateInstance<IAdService>();

        public long UserId { get; }

        public HomeViewModel(long userId) {
            UserId = userId;
        }

        public void Initialize()
        {
            UpdateUnreadNotificationCount();
            _adService.ArchiveExpiredAds();
        }

        public void UpdateUnreadNotificationCount()
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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
