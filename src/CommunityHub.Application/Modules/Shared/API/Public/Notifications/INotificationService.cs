using CommunityHub.Application.Helpers.Sorting;
using CommunityHub.Application.Modules.Shared.Core.Domain.Notifications;

namespace CommunityHub.Application.Modules.Shared.API.Public.Notifications
{
    public interface INotificationService
    {
        public void Create(Notification notification);
        public Notification MarkAsRead(Notification notification);

        public List<Notification> GetNotificationsForUser(
            long userId,
            NotificationOrder order = NotificationOrder.SentAtDescending,
            NotificationStatus? status = null
        );
        public int GetNotificationCountForUser(long userId, NotificationStatus? status = null);
    }
}
