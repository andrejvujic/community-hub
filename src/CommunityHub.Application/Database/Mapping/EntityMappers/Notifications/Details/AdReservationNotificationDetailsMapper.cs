using CommunityHub.Application.Domain.Notifications.Details;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers.Notifications.Details
{
    public class AdReservationNotificationDetailsMapper : EntityMapper<AdReservationNotificationDetails>
    {
        public override AdReservationNotificationDetails MapToEntity(IDataReader reader)
        {
            long id = reader.GetInt64(reader.GetOrdinal("details_id"));
            long notificationId = reader.GetInt64(reader.GetOrdinal("details_notification_id"));
            long adId = reader.GetInt64(reader.GetOrdinal("details_ad_id"));

            return new AdReservationNotificationDetails(
                id: id,
                adId: adId,
                notificationId: notificationId
            );
        }

        public override Dictionary<string, object?> MapToQueryParameters(AdReservationNotificationDetails entity)
        {
            return new() {
                { "details_notification_id", entity.NotificationId },
                { "details_ad_id", entity.AdId }
            };
        }
    }
}
