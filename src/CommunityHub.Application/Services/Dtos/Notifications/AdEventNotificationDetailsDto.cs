using CommunityHub.Application.Domain.Notifications.Details;

namespace CommunityHub.Application.Services.Dtos.Notifications
{
    public class AdEventNotificationDetailsDto : NotificationDetailsDto
    {
        public long AdId { get; }
        public AdEventType EventType { get; }

        public AdEventNotificationDetailsDto(
            long adId,
            AdEventType eventType,
            long notificationId = 0,
            long id = 0
        )
            : base(id, notificationId)
        {
            AdId = adId;
            EventType = eventType;
        }
    }
}
