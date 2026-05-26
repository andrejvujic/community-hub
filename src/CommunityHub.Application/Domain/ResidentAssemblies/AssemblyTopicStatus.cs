using System.Runtime.Serialization;

namespace CommunityHub.Application.Domain.ResidentAssemblies
{
    public enum AssemblyTopicStatus
    {
        [EnumMember(Value = "suggested")]
        Suggested,
        [EnumMember(Value = "accepted")]
        Accepted
    }
}
