using CommunityHub.Application.Domain.Notifications.Details;

namespace CommunityHub.Application.Database.Repositories.Notifications
{
    public class NotificationDetailsRepository
    {
        public void Create(NotificationDetails details)
        {
            switch (details)
            {
                case AdReservationNotificationDetails adReservationDetails:
                    CreateAdReservationDetails(adReservationDetails);
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
                    (@notification_id,
                    @ad_id)"
            )
            .AddParameter("notification_id", details.NotificationId)
            .AddParameter("ad_id", details.AdId)
            .Execute();
        }
    }
}
