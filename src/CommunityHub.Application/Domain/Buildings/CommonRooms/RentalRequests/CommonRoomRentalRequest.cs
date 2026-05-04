using CommunityHub.Application.Modules.Shared.Core.Domain.Users;

namespace CommunityHub.Application.Domain.Buildings.CommonRooms.RentalRequests
{ 
    public class CommonRoomRentalRequest
    {
        public long Id { get; private set; }
        public CommonRoom CommonRoom { get; private set; }
        public User Resident { get; private set; }

        public DateTime RequestedStartDate { get; private set; }
        public DateTime RequestedEndDate { get; private set; }
        public bool HasProposedDateRange => ProposedStartDate != null && ProposedEndDate != null;
        public DateTime? ProposedStartDate { get; private set; }
        public DateTime? ProposedEndDate { get; private set; }

        public DateTime? AssignedDate { get; private set; }

        public RentalRequestStatus Status { get; private set; }

        public CommonRoomRentalRequest(
            User resident,
            CommonRoom commonRoom,
            DateTime requestedStartDate,
            DateTime requestedEndDate,
            DateTime? proposedStartDate = null,
            DateTime? proposedEndDate = null,
            DateTime? assignedDate = null,
            RentalRequestStatus status = RentalRequestStatus.Waiting,
            long id = 0
        ) {
            Id = id;
            CommonRoom = commonRoom;
            Status = status;
            Resident = resident;

            RequestedStartDate = requestedStartDate;
            RequestedEndDate = requestedEndDate;
            ProposedStartDate = proposedStartDate;
            ProposedEndDate = proposedEndDate;
            AssignedDate = assignedDate;
        }

        public bool IsApproved => Status == RentalRequestStatus.Approved;
        public bool IsCancellable
            => Status == RentalRequestStatus.Waiting || Status == RentalRequestStatus.WaitingDateChange;  

        public void AcceptProposedDateRange()
        {
            RequestedStartDate = ProposedStartDate ?? RequestedStartDate;
            RequestedEndDate = ProposedEndDate ?? RequestedEndDate;
            ProposedStartDate = null;
            ProposedEndDate = null;
        }

        public CommonRoomRentalRequest CopyWith(
            User? resident = null,
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
                resident: resident ?? Resident,
                commonRoom: commonRoom ?? CommonRoom,
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