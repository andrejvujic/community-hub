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

        public List<Notification> GetNotificationsForUser(long userId, NotificationStatus? status = null)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                    notification.id AS notification_id,
                    notification.recipient_id,
                    notification.title,
                    notification.message,
                    notification.sent_at,
                    notification.type AS notification_type,
                    notification.status AS notification_status,

                    reservation_details.id  AS reservation_details_id,
                    reservation_details.notification_id AS reservation_details_notification_id,
                    reservation_details.ad_id AS reservation_details_ad_id

                FROM notifications notification

                LEFT OUTER JOIN ad_reservation_notification_details reservation_details ON
                    reservation_details.notification_id = notification.id
             
                WHERE
                    notification.recipient_id = @user_id AND
                    (notification.status = @status OR @status IS NULL)"
            )
            .AddParameter("user_id", userId)
            .AddParameter("status", status);

            List<Notification> notifications = new();

            query.ExecuteReader(reader => notifications.Add(
                    _notificationMapper.MapToEntity(reader)
                )
            );

            return notifications;
        }
    }
}
