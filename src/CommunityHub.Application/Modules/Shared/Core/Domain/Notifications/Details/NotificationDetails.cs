namespace CommunityHub.Application.Modules.Shared.Core.Domain.Notifications.Details
{
    public abstract class NotificationDetails
    {
        public long Id { get; protected set; }
        public long NotificationId { get; protected set; }

        public NotificationDetails(long id, long notificationId) {
            Id = id;
            NotificationId = notificationId;
        }

        public void AttachToNotification(long notificationId)
            => NotificationId = notificationId;
    }
}
