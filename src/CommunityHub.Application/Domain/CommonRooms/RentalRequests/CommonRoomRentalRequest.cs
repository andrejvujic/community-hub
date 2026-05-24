using CommunityHub.Application.Domain.CommonRooms;
using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.Domain.CommonRooms.RentalRequests
{ 
    public class CommonRoomRentalRequest
    {
        public long Id { get; }

        public long CommonRoomId { get; }
        public CommonRoom? CommonRoom { get; }

        public long ResidentId { get; }
        public User? Resident { get; }

        public DateTime RequestedStartDate { get; private set; }
        public DateTime RequestedEndDate { get; private set; }

        public bool HasProposedDateRange => ProposedStartDate is not null && ProposedEndDate is not null;
        public DateTime? ProposedStartDate { get; private set; }
        public DateTime? ProposedEndDate { get; private set; }

        public DateTime? AssignedDate { get; }

        public RentalRequestStatus Status { get; private set; }

        public CommonRoomRentalRequest(
            long residentId,
            long commonRoomId,
            DateTime requestedStartDate,
            DateTime requestedEndDate,
            User? resident = null,
            CommonRoom? commonRoom = null,
            DateTime? proposedStartDate = null,
            DateTime? proposedEndDate = null,
            DateTime? assignedDate = null,
            RentalRequestStatus status = RentalRequestStatus.Waiting,
            long id = 0
        ) {
            Id = id;

            CommonRoomId = commonRoomId;
            CommonRoom = commonRoom;

            ResidentId = residentId;
            Resident = resident;

            Status = status;

            RequestedStartDate = requestedStartDate;
            RequestedEndDate = requestedEndDate;
            ProposedStartDate = proposedStartDate;
            ProposedEndDate = proposedEndDate;
            AssignedDate = assignedDate;
        }

        public bool IsApproved => Status is RentalRequestStatus.Approved;
        public bool IsPending
            => Status is RentalRequestStatus.Waiting || Status is RentalRequestStatus.WaitingDateChange;  

        public void AcceptProposedDateRange()
        {
            RequestedStartDate = ProposedStartDate ?? RequestedStartDate;
            RequestedEndDate = ProposedEndDate ?? RequestedEndDate;
            ProposedStartDate = null;
            ProposedEndDate = null;

            Status = RentalRequestStatus.Waiting;
        }

        public CommonRoomRentalRequest CopyWith(
            long? residentId = null,
            User? resident = null,
            long? commonRoomId = null,
            CommonRoom? commonRoom = null,
            DateTime? requestedStartDate = null,
            DateTime? requestedEndDate = null,
            DateTime? proposedStartDate = null,
            DateTime? proposedEndDate = null,
            DateTime? assignedDate = null,
            RentalRequestStatus? status = null,
            long? id = null
        ) {
            return new CommonRoomRentalRequest(
                residentId: residentId ?? ResidentId,
                resident: resident ?? Resident,
                commonRoom: commonRoom ?? CommonRoom,
                commonRoomId: commonRoomId ?? CommonRoomId,
                requestedStartDate: requestedStartDate ?? RequestedStartDate,
                requestedEndDate: requestedEndDate ?? RequestedEndDate,
                proposedStartDate: proposedStartDate ?? ProposedStartDate,
                proposedEndDate: proposedEndDate ?? ProposedEndDate,
                status: status ?? Status,
                assignedDate: assignedDate ?? AssignedDate,
                id: id ?? Id
            );
        }
    }
}