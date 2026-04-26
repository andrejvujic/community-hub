using CommonRoomDomain = CommunityHub.Application.Domain.CommonRoom;

namespace CommunityHub.Application.Domain.CommonRoomRentalRequest
{ 
    public class CommonRoomRentalRequest
    {
        public long Id { get; private set; }
        public CommonRoomDomain.CommonRoom CommonRoom { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public RentalRequestStatus Status { get; private set; }
        public User Resident { get; private set; }
        public DateTime? AssignedDate { get; private set; }

        public CommonRoomRentalRequest(
            User resident,
            CommonRoomDomain.CommonRoom commonRoom,
            DateTime startDate,
            DateTime endDate,
            RentalRequestStatus status = RentalRequestStatus.Waiting,
            DateTime? assignedDate = null,
            long id = 0
        ) {
            Id = id;
            CommonRoom = commonRoom;
            Status = status;
            Resident = resident;
            AssignedDate = assignedDate;

            if (startDate > endDate || (assignedDate.HasValue && assignedDate < startDate) || (assignedDate.HasValue && assignedDate > endDate)) 
                throw new ArgumentNullException("Invalid date range.");

            StartDate = startDate;
            EndDate = endDate;
        }

        public CommonRoomRentalRequest CopyWith(
            CommonRoomDomain.CommonRoom? commonRoom = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            RentalRequestStatus? status = null,
            User? resident = null,
            DateTime? assignedDate = null,
            long? id = null
        ) {
            return new CommonRoomRentalRequest(
                resident ?? Resident,
                commonRoom ?? CommonRoom,
                startDate ?? StartDate,
                endDate ?? EndDate,
                status ?? Status,
                assignedDate ?? AssignedDate,
                id ?? Id
            );
        }
    }
}