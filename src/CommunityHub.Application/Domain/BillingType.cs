using System.Runtime.Serialization;

namespace CommunityHub.Application.Domain
{
    public enum BillingType
    {
        [EnumMember(Value = "per_hour")]
        PerHour,
        [EnumMember(Value = "per_job")]
        PerJob
    }
}