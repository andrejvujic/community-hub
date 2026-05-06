using CommunityHub.Application.Database;
using CommunityHub.Application.Database.Mapping.EntityMappers.Notifications;
using CommunityHub.Application.Helpers.Sorting;
using CommunityHub.Application.Modules.Shared.Core.Domain.Notifications;
using CommunityHub.Application.Modules.Shared.Core.Domain.Notifications.RepositoryInterfaces;

namespace CommunityHub.Application.Shared.Database.Repositories.Notifications
{
    public class NotificationRepository: INotificationRepository
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
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
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
            .ExtendParameters(
                _notificationMapper.MapToQueryParameters(notification)
            );

            long notificationId = connection.ExecuteScalar(query, Convert.ToInt64);

            if (!notification.HasDetails) return;

            var details = notification.Details!;
            details.AttachToNotification(notificationId);
            _notificationDetailsRepository.Create(details);
        }

        public void CreateMany(List<Notification> notifications)
        {
            using var connection = DatabaseConnection.Create();

            foreach (var notification in notifications)
            {
                var query = DatabaseQuery.Create(
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
                .ExtendParameters(
                    _notificationMapper.MapToQueryParameters(notification)
                );

                long notificationId = connection.ExecuteScalar(query, Convert.ToInt64);

                if (!notification.HasDetails) return;

                notification.Details!.AttachToNotification(notificationId);
            }

            _notificationDetailsRepository.CreateMany(
                notifications
                    .Where(notification => notification.HasDetails)
                    .Select(notification => notification.Details!)
                    .ToList()
            );
        }

        public void Update(Notification notification)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
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
            .ExtendParameters(
                _notificationMapper.MapToQueryParameters(notification)
            );

            connection.Execute(query);
        }

        public List<Notification> GetByUserId(
            long userId,
            NotificationOrder order = NotificationOrder.SentAtDescending,
            NotificationStatus? status = null
        )
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
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

            connection.ExecuteReader(
                query,
                reader => {
                    notifications.Add(
                        _notificationMapper.MapToEntity(reader)
                    );
                }
            );

            return notifications;
        }
    }
}
