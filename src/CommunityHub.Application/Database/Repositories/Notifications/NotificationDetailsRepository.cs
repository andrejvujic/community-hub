using CommunityHub.Application.Database.Mapping.EntityMappers.Notifications.Details;
using CommunityHub.Application.Domain.Notifications.Details;

namespace CommunityHub.Application.Database.Repositories.Notifications
{
    public class NotificationDetailsRepository
    {
        private readonly AdReservationNotificationDetailsMapper _reservationDetailsMapper = new();

        public void Create(NotificationDetails details)
        {
            switch (details)
            {
                case AdReservationNotificationDetails reservationDetails:
                    CreateAdReservationDetails(reservationDetails);
                    break;
            }
        }

        private void CreateAdReservationDetails(AdReservationNotificationDetails details)
        {
            PostgresQuery.Create(
                @"INSERT INTO ad_reservation_notification_details
                    (notification_id,
                    ad_id)
                VALUES
                    (@details_notification_id,
                    @details_ad_id)"
            )
            .ExtendParameters(_reservationDetailsMapper.MapToQueryParameters(details))
            .Execute();
        }
    }
}
