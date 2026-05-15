using System.Runtime.Serialization;

namespace CommunityHub.Application.Domain.Neighborhoods
{
    public enum NeighborhoodAccessRequestStatus
    {
        [EnumMember(Value = "accepted")]
        Accepted,
        [EnumMember(Value = "rejected")]
        Rejected,
        [EnumMember(Value = "waiting")]
        Waiting
    }
}
