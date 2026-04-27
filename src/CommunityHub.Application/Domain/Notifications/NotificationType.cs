using System.Runtime.Serialization;

namespace CommunityHub.Application.Domain.Notifications
{
    public enum NotificationType
    {
        [EnumMember(Value = "other")]
        Other,
        [EnumMember(Value = "ad_event")]
        AdEvent,
    }
}
