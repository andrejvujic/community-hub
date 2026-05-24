using CommunityHub.Application.Domain.CommonRooms.RentalRequests;
using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.Services.Dtos.CommonRooms
{
    public class CommonRoomRentalRequestDto
    {
        public long Id { get; set; }

        public long CommonRoomId { get; set; }
        public CommonRoomDto? CommonRoom { get; set; }

        public long ResidentId { get; set; }
        public UserDto? Resident { get; set; }

        public DateTime RequestedStartDate { get; set; }
        public DateTime RequestedEndDate { get; set; }

        public DateTime? ProposedStartDate { get; set; }
        public DateTime? ProposedEndDate { get; set; }

        public DateTime? AssignedDate { get; set; }

        public RentalRequestStatus Status { get; set; }

        public CommonRoomRentalRequestDto(
            long commonRoomId,
            long residentId,
            DateTime requestedStartDate,
            DateTime requestedEndDate,
            RentalRequestStatus status,
            CommonRoomDto? commonRoom = null,
            UserDto? resident = null,
            DateTime? assignedDate = null,
            DateTime? proposedStartDate = null,
            DateTime? proposedEndDate = null,
            long id = 0
        )
        {
            Id = id;
            CommonRoomId = commonRoomId;
            CommonRoom = commonRoom;
            ResidentId = residentId;
            Resident = resident;
            RequestedStartDate = requestedStartDate;
            RequestedEndDate = requestedEndDate;
            ProposedStartDate = proposedStartDate;
            ProposedEndDate = proposedEndDate;
            AssignedDate = assignedDate;
            Status = status;
        }

        public bool IsApproved => Status is RentalRequestStatus.Approved;
        public bool IsPending
            => Status is RentalRequestStatus.Waiting || Status is RentalRequestStatus.WaitingDateChange;

        public bool HasProposedDateRange => ProposedStartDate is not null && ProposedEndDate is not null;
    }
}