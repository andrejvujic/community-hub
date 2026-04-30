using System.Runtime.Serialization;

namespace CommunityHub.Application.Domain.Buildings.Issues
{
    public enum IssueStatus
    {
        [EnumMember(Value = "open")]
        Open,
        [EnumMember(Value = "potentially_resolved")]
        PotentiallyResolved,
        [EnumMember(Value = "resolved")]
        Resolved,
    }
}