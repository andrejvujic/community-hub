using System.Runtime.Serialization;

namespace CommunityHub.Application.Domain.CommonRoom
{
    public enum RentalType
    {
        [EnumMember(Value = "one_day")]
        OneDay,
        [EnumMember(Value = "multi_day")]
        MultiDay,
    }
}