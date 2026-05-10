using System.Runtime.Serialization;

namespace CommunityHub.Application.Domain.NeighborhoodMeetings
{
    public enum MeetingTheme
    {
        [EnumMember(Value = "welcome")]
        Welcome,
        [EnumMember(Value = "motivation")]
        Motivation
    }
}