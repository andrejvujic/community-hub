using CommunityHub.Application.Database.Mapping.EntityMappers.Notifications.Details;
using CommunityHub.Application.Domain.Notifications;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers.Notifications
{
    public class NotificationMapper : EntityMapper<Notification>
    {
        private readonly UserMapper _userMapper = new();

        private readonly AdReservationNotificationDetailsMapper _reservationDetailsMapper = new();

        public override Notification MapToEntity(IDataReader reader)
        {
            var id = reader.GetInt64(reader.GetOrdinal("notification_id"));

            var recipientId = reader.GetInt64(reader.GetOrdinal("recipient_id"));
            var recipient = _userMapper.MapToEntity(reader.WithPrefix("recipient"));

            var sentAt = reader.GetDateTime(reader.GetOrdinal("sent_at"));

            var title = reader.GetString(reader.GetOrdinal("title"));
            var message = reader.GetString(reader.GetOrdinal("message"));

            var status = (NotificationStatus)reader.GetValue(reader.GetOrdinal("notification_status"));
            var type = (NotificationType)reader.GetValue(reader.GetOrdinal("notification_type"));

            var details = type switch
            {
                NotificationType.AdReservation => _reservationDetailsMapper.MapToEntity(
                    reader.WithPrefix("reservation")
                ),
                _ => null
            };

            return new Notification(
                id: id,
                recipientId: recipientId,
                recipient: recipient,
                title: title,
                message: message,
                status: status,
                type: type,
                sentAt: sentAt,
                details: details
            );
        }

        public override Dictionary<string, object?> MapToQueryParameters(Notification entity)
        {
            return new()
            {
                { "recipient_id", entity.RecipientId },
                { "sent_at", entity.SentAt },
                { "title", entity.Title },
                { "message", entity.Message },
                { "type", entity.Type },
                { "status", entity.Status }
            };
        }
    }
}
