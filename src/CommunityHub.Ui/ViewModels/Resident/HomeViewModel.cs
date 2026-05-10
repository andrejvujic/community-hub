using CommunityHub.Application;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.RepositoryInterfaces;
using CommunityHub.Application.Modules.Buildings.Core.Services.Ads;
using CommunityHub.Application.Modules.Shared.Core.Domain.Notifications;
using CommunityHub.Application.Modules.Shared.Core.Domain.Notifications.RepositoryInterfaces;
using CommunityHub.Application.Modules.Shared.Core.Services.Notifications;
using System.ComponentModel;

namespace CommunityHub.Ui.ViewModels.Resident
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private readonly NotificationService _notificationService;
        private readonly AdService _adService;

        public long UserId { get; }

        public HomeViewModel(long userId) {
            _notificationService = new(
                Injector.CreateInstance<INotificationRepository>()
            );

            _adService = new(
                Injector.CreateInstance<IAdRepository>(),
                Injector.CreateInstance<IAdTimeSlotRepository>()
            );

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
