using System.Runtime.Serialization;

namespace CommunityHub.Application.Modules.Buildings.Core.Domain.Ads
{
    public enum AdStatus
    {
        [EnumMember(Value = "active")]
        Active,
        [EnumMember(Value = "archived")]
        Archived
    }
}
