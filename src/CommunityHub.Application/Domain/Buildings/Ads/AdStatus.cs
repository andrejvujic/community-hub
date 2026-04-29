using System.Runtime.Serialization;

namespace CommunityHub.Application.Domain.Buildings.Ads
{
    public enum AdStatus
    {
        [EnumMember(Value = "active")]
        Active,
        [EnumMember(Value = "archived")]
        Archived
    }
}
