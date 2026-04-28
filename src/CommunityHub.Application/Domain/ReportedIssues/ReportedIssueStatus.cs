using System.Runtime.Serialization;

namespace CommunityHub.Application.Domain.ReportedIsssue
{
    public enum ReportedIssueStatus
    {
        [EnumMember(Value = "open")]
        Open,
        [EnumMember(Value = "potentially_resolved")]
        PotentiallyResolved,
        [EnumMember(Value = "resolved")]
        Resolved,
    }
}