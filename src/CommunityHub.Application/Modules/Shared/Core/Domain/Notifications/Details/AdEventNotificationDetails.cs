using System.Runtime.Serialization;

namespace CommunityHub.Application.Modules.Shared.Core.Domain.Notifications.Details
{
    public enum AdEventType {
        [EnumMember(Value = "matching_ad_created")]
        MatchingAdCreated,
        [EnumMember(Value = "ad_time_slots_reserved")]
        AdTimeSlotsReserved
    }

    public class AdEventNotificationDetails : NotificationDetails
    {
        public long AdId { get; }
        public AdEventType EventType { get; }

        public AdEventNotificationDetails(
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

        public AdEventNotificationDetails CopyWith(
            long? adId = null,
            AdEventType? eventType = null,
            long? notificationId = null,
            long? id = null
        ) => new AdEventNotificationDetails(
            adId ?? AdId,
            eventType ?? EventType,
            notificationId ?? NotificationId,
            id ?? Id
        );
    }
}
