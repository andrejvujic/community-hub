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
            long id,
            long commonRoomId,
            CommonRoomDto? commonRoom,
            long residentId,
            UserDto? resident,
            DateTime requestedStartDate,
            DateTime requestedEndDate,
            DateTime? proposedStartDate,
            DateTime? proposedEndDate,
            DateTime? assignedDate,
            RentalRequestStatus status
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
    }
}