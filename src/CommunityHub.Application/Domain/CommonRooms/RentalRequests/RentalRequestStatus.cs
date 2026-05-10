using System.Runtime.Serialization;

namespace CommunityHub.Application.Domain.CommonRooms.RentalRequests
{
    public enum RentalRequestStatus
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