
namespace CommunityHub.Application.Domain.CommonRooms.RentalRequests
{
    public class CommonRoomRentalRequestsReport
    {
        public CommonRoom CommonRoom { get; }
        public IReadOnlyList<CommonRoomRentalRequest> RentalRequests { get; }

        public int TotalRequestsCount => RentalRequests.Count;   
        public int ApprovedRequestsCount { get; }
        public int PendingRequestsCount { get; }
        public int RejectedRequestsCount { get; }

        public CommonRoomRentalRequestsReport(
            CommonRoom commonRoom, 
            List<CommonRoomRentalRequest> rentalRequests
        )
        {
            CommonRoom = commonRoom;
            RentalRequests = rentalRequests.AsReadOnly();

            ApprovedRequestsCount = rentalRequests.Count(r => r.IsApproved);
            PendingRequestsCount = rentalRequests.Count(r => r.IsPending);
            RejectedRequestsCount = TotalRequestsCount - ApprovedRequestsCount - PendingRequestsCount;
        }
    }
}
