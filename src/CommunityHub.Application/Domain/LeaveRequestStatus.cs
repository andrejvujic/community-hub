using System.Runtime.Serialization;

namespace CommunityHub.Application.Domain
{
    public enum LeaveRequestStatus
    {
        [EnumMember(Value = "pending_approval")]
        PendingApproval,
        [EnumMember(Value = "approved")]
        Approved,
        [EnumMember(Value = "auto_approved")]
        AutoApproved,
        [EnumMember(Value = "rejected")]
        Rejected
    }
}
