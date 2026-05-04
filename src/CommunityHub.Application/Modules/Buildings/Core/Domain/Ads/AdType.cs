using System.Runtime.Serialization;

namespace CommunityHub.Application.Modules.Buildings.Core.Domain.Ads
{
    public enum AdType
    {
        [EnumMember(Value = "offer")]
        Offer,
        [EnumMember(Value = "request")]
        Request,
    }

    public static class AdTypeValues
    {
        public static Array All => Enum.GetValues(typeof(AdType));
    }
}
