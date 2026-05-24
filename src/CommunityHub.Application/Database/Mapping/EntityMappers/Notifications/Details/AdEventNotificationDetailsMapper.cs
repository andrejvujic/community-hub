using CommunityHub.Application.Domain.Notifications.Details;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers.Notifications.Details
{
    public class AdEventNotificationDetailsMapper : EntityMapper<AdEventNotificationDetails>
    {
        public override AdEventNotificationDetails MapToEntity(IDataReader reader)
        {
            long id = reader.GetInt64(reader.GetOrdinal("details_id"));
            long notificationId = reader.GetInt64(reader.GetOrdinal("details_notification_id"));

            long adId = reader.GetInt64(reader.GetOrdinal("details_ad_id"));
            AdEventType eventType = (AdEventType)reader.GetValue(
                reader.GetOrdinal("details_ad_event_type")
            );

            return new AdEventNotificationDetails(
                id: id,
                notificationId: notificationId,
                adId: adId,
                eventType: eventType
            );
        }

        public override Dictionary<string, object?> MapToQueryParameters(AdEventNotificationDetails entity)
        {
            return new() {
                { "details_notification_id", entity.NotificationId },
                { "details_ad_id", entity.AdId },
                { "details_ad_event_type", entity.EventType }
            };
        }
    }
}
