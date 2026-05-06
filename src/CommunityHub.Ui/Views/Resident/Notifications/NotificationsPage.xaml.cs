using CommunityHub.Application;
using CommunityHub.Application.Modules.Shared.API.Public.Notifications;
using CommunityHub.Application.Modules.Shared.Core.Domain.Notifications;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Notifications.Details;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Notifications
{
    public partial class NotificationsPage : MainNavigatorConsumerPage, INotifyPropertyChanged
    {
        private readonly long _userId;
        private readonly INotificationService _noticationService = Injector.CreateInstance<INotificationService>();

        public ObservableCollection<Notification> Notifications { get; } = new();
        public bool HasNotifications => Notifications.Count > 0;

        public NotificationsPage(long userId)
        {
            InitializeComponent();
            DataContext = this;

            _userId = userId;

            UpdateNotifications();
        }

        private void UpdateNotifications()
        {
            Notifications.Clear();

            var notifications = _noticationService.GetNotificationsForUser(_userId);
            foreach (var notification in notifications)
                Notifications.Add(notification);

            OnPropertyChanged(nameof(HasNotifications));
        }

        private void OpenNotificationDetailsPage(Notification notification)
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

            if (element.DataContext is not Notification notification) return;

            if (!notification.IsRead)
                _noticationService.MarkAsRead(notification);

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
