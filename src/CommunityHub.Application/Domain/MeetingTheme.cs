using System.Runtime.Serialization;

namespace CommunityHub.Application.Domain
{
    public enum MeetingTheme
    {
        [EnumMember(Value = "welcome")]
        Welcome,
        [EnumMember(Value = "motivation")]
        Motivation
    }
}