using CommunityHub.Application.Database.Repositories.Notifications;
using CommunityHub.Application.Domain.Notifications;
using CommunityHub.Ui.Helpers.Navigation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Notifications
{
    public partial class NotificationsPage : MainNavigatorConsumerPage, INotifyPropertyChanged
    {
        private readonly long _userId;
        private readonly NotificationRepository _notificationRepository = new();

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

            var notifications = _notificationRepository.GetNotificationsForUser(_userId);
            foreach (var notification in notifications)
                Notifications.Add(notification);

            OnPropertyChanged(nameof(HasNotifications));
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
            => Pop();

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
