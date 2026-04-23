using System.Runtime.Serialization;

namespace CommunityHub.Application.Domain.CommonRoomRentalRequest
{
    public enum RentalStatus
    {
        [EnumMember(Value = "approved")]
        Approved,
        [EnumMember(Value = "waiting")]
        Waiting,
        [EnumMember(Value = "rejected")]
        Rejected,
        [EnumMember(Value = "waiting_date_change")]
        WaitingDateChange
    }
}