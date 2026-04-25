namespace CommunityHub.Application.Domain.Notifications.Details
{
    public class AdReservationNotificationDetails : NotificationDetails
    {
        public long AdId { get; }

        public AdReservationNotificationDetails(long id, long notificationId, long adId)
            : base(id, notificationId)
        {
            AdId = adId;    
        }
    }
}
