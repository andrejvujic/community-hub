using System.Runtime.Serialization;

namespace CommunityHub.Application.Domain.Notifications
{
    public enum NotificationType
    {
        [EnumMember(Value = "ad_reservation")]
        AdReservation,
        [EnumMember(Value = "ad_creation")]
        AdCreation,
    }
}
