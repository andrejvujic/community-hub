using System.Runtime.Serialization;

namespace CommunityHub.Application.Domain
{
    public enum ParityType
    {
        [EnumMember(Value = "even")]
        Even,
        [EnumMember(Value = "odd")]
        Odd,
        [EnumMember(Value = "both")]
        Both
    }
}
