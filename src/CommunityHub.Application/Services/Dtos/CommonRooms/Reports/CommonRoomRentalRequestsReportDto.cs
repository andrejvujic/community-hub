using CommunityHub.Application.Services.Dtos.CommonRooms;

namespace CommunityHub.Application.Services.Dtos.CommonRooms.Reports
{
    public class CommonRoomRentalRequestsReportDto
    {
        public CommonRoomDto CommonRoom { get; }
        public IReadOnlyList<CommonRoomRentalRequestDto> RentalRequests { get; }

        public int TotalRequestsCount => RentalRequests.Count;
        public int ApprovedRequestsCount { get; }
        public int PendingRequestsCount { get; }
        public int RejectedRequestsCount { get; }

        public CommonRoomRentalRequestsReportDto(
            CommonRoomDto commonRoom,
            List<CommonRoomRentalRequestDto> rentalRequests
        )
        {
            CommonRoom = commonRoom;
            RentalRequests = rentalRequests.AsReadOnly();

            ApprovedRequestsCount = rentalRequests.Count(request => request.IsApproved);
            PendingRequestsCount = rentalRequests.Count(request => request.IsPending);
            RejectedRequestsCount = TotalRequestsCount - ApprovedRequestsCount - PendingRequestsCount;
        }
    }
}
