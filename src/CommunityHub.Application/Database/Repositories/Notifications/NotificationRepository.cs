using CommunityHub.Application.Database.Mapping.EntityMappers.Notifications;
using CommunityHub.Application.Domain.Notifications;
using CommunityHub.Application.Helpers.Sorting;

namespace CommunityHub.Application.Database.Repositories.Notifications
{
    public class NotificationRepository
    {
        private readonly NotificationMapper _notificationMapper = new(); 
        private readonly NotificationDetailsRepository _notificationDetailsRepository = new();

        private static string GetOrderByClause(NotificationOrder order)
        {
            return order switch
            {
                NotificationOrder.SentAtAscending => "ORDER BY notification.sent_at ASC",
                NotificationOrder.SentAtDescending => "ORDER BY notification.sent_at DESC",
                _ => string.Empty
            };
        }

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

        public void Update(Notification notification)
        {
            PostgresQuery.Create(
                @"UPDATE notifications SET
                    recipient_id = @recipient_id,
                    sent_at = @sent_at,
                    title = @title,
                    message = @message,
                    status = @status,
                    type = @type
                WHERE id = @id"
            )
            .AddParameter("id", notification.Id)
            .ExtendParameters(_notificationMapper.MapToQueryParameters(notification))
            .Execute();
        }

        public int GetCountByUserId(
            long userId,
            NotificationStatus? status = null
        )
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

        public List<Notification> GetByUserId(
            long userId,
            NotificationOrder order = NotificationOrder.SentAtDescending,
            NotificationStatus? status = null
        )
        {
            var query = PostgresQuery.Create(
                $@"SELECT
                    notification.id AS notification_id,
                    notification.recipient_id,
                    notification.title,
                    notification.message,
                    notification.sent_at,
                    notification.type AS notification_type,
                    notification.status AS notification_status,

                    ad_event.id  AS ad_event_details_id,
                    ad_event.notification_id AS ad_event_details_notification_id,
                    ad_event.ad_id AS ad_event_details_ad_id,
                    ad_event.ad_event_type AS ad_event_details_ad_event_type

                FROM notifications notification

                LEFT OUTER JOIN ad_event_notification_details ad_event ON
                    ad_event.notification_id = notification.id
             
                WHERE
                    notification.recipient_id = @user_id AND
                    (notification.status = @status OR @status IS NULL)
                
               {GetOrderByClause(order)}"
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
