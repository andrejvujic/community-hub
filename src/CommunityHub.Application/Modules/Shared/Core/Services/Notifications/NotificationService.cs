using CommunityHub.Application.Helpers.Sorting;
using CommunityHub.Application.Modules.Shared.API.Public.Notifications;
using CommunityHub.Application.Modules.Shared.Core.Domain.Notifications;
using CommunityHub.Application.Modules.Shared.Core.Domain.Notifications.RepositoryInterfaces;

namespace CommunityHub.Application.Modules.Shared.Core.Services.Notifications
{
    public class NotificationService: INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public void Create(Notification notification) => _notificationRepository.Create(notification);

        public List<Notification> GetNotificationsForUser(
            long userId,
            NotificationOrder order = NotificationOrder.SentAtDescending,
            NotificationStatus? status = null
        ) => _notificationRepository.GetByUserId(
                userId,
                order: order,
                status: status
            );

        public int GetNotificationCountForUser(long userId, NotificationStatus? status = null)
        {
            var notifications = _notificationRepository.GetByUserId(
                userId,
                status: status
            );
            return notifications.Count;
        }

        public Notification MarkAsRead(Notification notification)
        {
            notification.MarkAsRead();
            _notificationRepository.Update(notification);
            return notification;
        }
    }
}
