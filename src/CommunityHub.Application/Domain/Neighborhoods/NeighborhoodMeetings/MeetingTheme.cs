using System.Runtime.Serialization;

namespace CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings
{
    public enum MeetingTheme
    {
        [EnumMember(Value = "welcome")]
        Welcome,
        [EnumMember(Value = "motivation")]
        Motivation
    }
}