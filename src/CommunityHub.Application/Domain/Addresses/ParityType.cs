using System.Runtime.Serialization;

namespace CommunityHub.Application.Domain.Addresses
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
