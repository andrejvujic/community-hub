using CommunityHub.Application;
using CommunityHub.Application.Domain.Notifications;
using CommunityHub.Application.Domain.Notifications.RepositoryInterfaces;
using CommunityHub.Application.Services.Dtos.Notifications;
using CommunityHub.Application.Services.Notifications;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Notifications.Details;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Notifications
{
    public partial class NotificationsPage : MainNavigationConsumerPage, INotifyPropertyChanged
    {
        private readonly long _userId;
        private readonly NotificationService _notificationService;

        public ObservableCollection<NotificationDto> Notifications { get; } = new();
        public bool HasNotifications => Notifications.Count > 0;

        public NotificationsPage(long userId)
        {
            _notificationService = new(
                Injector.CreateInstance<INotificationRepository>()
            );

            InitializeComponent();
            DataContext = this;

            _userId = userId;

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

        private void OpenNotificationDetailsPage(NotificationDto notification)
        {
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

        private void NotificationItem_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is not FrameworkElement element) return;

            if (element.DataContext is not NotificationDto notification) return;

            if (!notification.IsRead)
                _notificationService.MarkAsRead(notification);

            if (notification.HasDetails)
                OpenNotificationDetailsPage(notification);
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
