using System.Runtime.Serialization;

namespace CommunityHub.Application.Domain.Events
{
    public enum EventStatus
    {
        [EnumMember(Value = "preparation")]
        Preparation,
        [EnumMember(Value = "scheduled")]
        Scheduled,
        [EnumMember(Value = "finished")]
        Finished,
        [EnumMember(Value = "cancelled")]
        Cancelled
    }
}
