using System.Runtime.Serialization;

namespace CommunityHub.Application.Domain.TrustRecords
{
    public enum TrustStatus
    {
        [EnumMember(Value = "new")]
        New,
        [EnumMember(Value = "inactive")]
        Inactive,
        [EnumMember(Value = "active")]
        Active,
        [EnumMember(Value = "prominent")]
        Prominent,
        [EnumMember(Value = "trustworthy")]
        Trustworthy
    }
}
