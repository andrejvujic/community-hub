using CommunityHub.Application.Domain.Notifications.Details;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers.Notifications.Details
{
    public class AdEventNotificationDetailsMapper : EntityMapper<AdEventNotificationDetails>
    {
        public override AdEventNotificationDetails? MapToEntity(IDataReader reader)
        {
            if (!reader.HasColumn("details_id")) return null;
            var idColumnOrdinal = reader.GetOrdinal("details_id");

            if (reader.IsDBNull(idColumnOrdinal)) return null;
            var id = reader.GetInt64(reader.GetOrdinal("details_id"));

            var notificationId = reader.GetInt64(reader.GetOrdinal("details_notification_id"));

            var adId = reader.GetInt64(reader.GetOrdinal("details_ad_id"));
            var eventType = (AdEventType)reader.GetValue(
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
