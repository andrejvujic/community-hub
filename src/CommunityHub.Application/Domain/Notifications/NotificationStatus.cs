using System.Runtime.Serialization;

namespace CommunityHub.Application.Domain.Notifications
{
    public enum NotificationStatus
    {
        [EnumMember(Value = "read")]
        Read,
        [EnumMember(Value = "unread")]
        Unread
    }
}
