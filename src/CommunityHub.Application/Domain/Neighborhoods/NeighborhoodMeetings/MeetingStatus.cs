using System.Runtime.Serialization;

namespace CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings
{
    public enum MeetingStatus
    {
        [EnumMember(Value = "in_preparation")]
        InPreparation,
        [EnumMember(Value = "scheduled")]
        Scheduled,
        [EnumMember(Value = "cancelled")]
        Cancelled
    }
}