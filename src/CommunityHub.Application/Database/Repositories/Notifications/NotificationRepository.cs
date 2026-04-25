using CommunityHub.Application.Domain.Notifications;

namespace CommunityHub.Application.Database.Repositories.Notifications
{
    public class NotificationRepository
    {
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
