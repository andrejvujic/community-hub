namespace CommunityHub.Application.Domain.Notifications.Details
{
    public class AdReservationNotificationDetails : NotificationDetails
    {
        public long AdId { get; }

        public AdReservationNotificationDetails(long adId, long notificationId = 0, long id = 0)
            : base(id, notificationId)
        {
            AdId = adId;    
        }

        public AdReservationNotificationDetails CopyWith(
            long? adId = null,
            long? notificationId = null,
            long? id = null
        ) => new AdReservationNotificationDetails(
            adId ?? AdId,
            notificationId ?? NotificationId,
            id ?? Id
        );
    }
}
