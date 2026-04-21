using System.Runtime.Serialization;

namespace CommunityHub.Application.Domain.NoticeBoard
{
    public enum AdStatus
    {
        [EnumMember(Value = "active")]
        Active,
        [EnumMember(Value = "archived")]
        Archived
    }
}
