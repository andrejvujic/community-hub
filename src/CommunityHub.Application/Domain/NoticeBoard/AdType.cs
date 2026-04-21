using System.Runtime.Serialization;

namespace CommunityHub.Application.Domain.NoticeBoard
{
    public enum AdType
    {
        [EnumMember(Value = "offer")]
        Offer,
        [EnumMember(Value = "request")]
        Request,
    }
}
