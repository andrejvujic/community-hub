using CommunityHub.Application;
using CommunityHub.Application.Domain.Notifications;
using CommunityHub.Application.Domain.Notifications.RepositoryInterfaces;
using CommunityHub.Application.Services.Dtos.Notifications;
using CommunityHub.Application.Services.Notifications;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Notifications.Details;
using System.Collections.ObjectModel;

namespace CommunityHub.Ui.ViewModels.Resident.Notifications
{
    public class NotificationsViewModel : BaseViewModel
    {
        private readonly long _userId;
        private readonly NotificationService _notificationService;

        public ObservableCollection<NotificationDto> Notifications { get; } = new();
        public bool HasNotifications => Notifications.Count > 0;
        public RelayCommand<NotificationDto> OpenNotificationDetailsCommand { get; }

        public NotificationsViewModel(long userId, IMainNavigationConsumer navigationConsumer)
            : base(navigationConsumer)
        {
            _notificationService = new(
                Injector.CreateInstance<INotificationRepository>()
            );

            _userId = userId;
            OpenNotificationDetailsCommand = new(OpenNotificationDetailsPage);

            UpdateNotifications();
        }

        private void UpdateNotifications()
        {
            Notifications.Clear();

            var notifications = _notificationService.GetNotificationsForUser(_userId);
            foreach (var notification in notifications)
                Notifications.Add(notification);

            OnPropertyChanged(nameof(HasNotifications));
        }

        private void OpenNotificationDetailsPage(NotificationDto? notification)
        {
            if (notification is null) return;

            if (!notification.IsRead)
                _notificationService.MarkAsRead(notification);

            if (!notification.HasDetails)
            {
                UpdateNotifications();
                return;
            }

            switch (notification.Type)
            {
                case NotificationType.AdEvent:
                    Push(
                        new AdEventNotificationDetailsPage(_userId, notification),
                        onPop: _ => UpdateNotifications()
                    );
                    break;
            }
        }
    }
}
