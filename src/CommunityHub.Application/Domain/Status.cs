using System.Runtime.Serialization;

namespace CommunityHub.Application.Domain
{
    public enum Status
    {
        [EnumMember(Value = "accepted")]
        Accepted,
        [EnumMember(Value = "rejected")]
        Rejected,
        [EnumMember(Value = "waiting")]
        Waiting
    }
}
