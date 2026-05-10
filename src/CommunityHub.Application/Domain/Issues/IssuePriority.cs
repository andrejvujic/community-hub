using System.Runtime.Serialization;

namespace CommunityHub.Application.Domain.Issues
{
    public enum IssuePriority
    {
        [EnumMember(Value = "can_wait")]
        CanWait,
        [EnumMember(Value = "should_be_resolved_soon")]
        ShouldBeResolvedSoon,
        [EnumMember(Value = "urgent")]
        Urgent
    }
}