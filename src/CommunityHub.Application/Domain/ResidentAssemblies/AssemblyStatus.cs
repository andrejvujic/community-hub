using System.Runtime.Serialization;

namespace CommunityHub.Application.Domain.ResidentAssemblies
{
    public enum AssemblyStatus
    {
        [EnumMember(Value = "scheduled")]
        Scheduled,
        [EnumMember(Value = "confirmed")]
        Confirmed,
        [EnumMember(Value = "cancelled")]
        Cancelled
    }
}
