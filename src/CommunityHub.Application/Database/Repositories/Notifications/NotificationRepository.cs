using CommunityHub.Application.Database.Mapping.EntityMappers.Notifications;
using CommunityHub.Application.Domain.Notifications;

namespace CommunityHub.Application.Database.Repositories.Notifications
{
    public class NotificationRepository
    {
        private readonly NotificationMapper _notificationMapper = new(); 
        private readonly NotificationDetailsRepository _notificationDetailsRepository = new NotificationDetailsRepository();

        public void Create(Notification notification)
        {
            var query = PostgresQuery.Create(
                @"INSERT INTO notifications
                    (recipient_id,
                    sent_at,
                    title,
                    message,
                    status,
                    type)
                VALUES
                    (@recipient_id,
                    @sent_at,
                    @title,
                    @message,
                    @status,
                    @type)
                RETURNING id"
            )
            .ExtendParameters(_notificationMapper.MapToQueryParameters(notification));

            long notificationId = query.ExecuteScalar<long>(value => Convert.ToInt64(value));

            if (!notification.HasDetails) return;

            var details = notification.Details!;
            details.AttachToNotification(notificationId);
            _notificationDetailsRepository.Create(details);
        }

        public int GetNotificationCountForUser(long userId, NotificationStatus? status = null)
        {
            var query = PostgresQuery.Create(
                @"SELECT COUNT(*) notification_count FROM notifications notification
                INNER JOIN users recipient ON recipient.id = notification.recipient_id
                WHERE
                    recipient.id = @user_id AND
                    (notification.status = @status OR @status IS NULL)"
            )
            .AddParameter("user_id", userId)
            .AddParameter("status", status);

            return query.ExecuteScalar<int>(value => Convert.ToInt32(value));
        }
    }
}
