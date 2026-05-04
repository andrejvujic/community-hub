using System.Runtime.Serialization;

namespace CommunityHub.Application.Modules.Shared.Core.Domain.Notifications
{
    public enum NotificationStatus
    {
        [EnumMember(Value = "read")]
        Read,
        [EnumMember(Value = "unread")]
        Unread
    }
}
