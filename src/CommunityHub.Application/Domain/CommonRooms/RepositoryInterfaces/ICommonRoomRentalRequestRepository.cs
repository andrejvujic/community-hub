using CommunityHub.Application.Domain.CommonRooms.RentalRequests;

namespace CommunityHub.Application.Domain.CommonRooms.RepositoryInterfaces
{
    public interface ICommonRoomRentalRequestRepository
    {
        public void Create(CommonRoomRentalRequest request);
        public void Delete(long id);
        public void Update(CommonRoomRentalRequest request);

        public List<CommonRoomRentalRequest> GetAllByBuildingId(long buildingId);
        public List<CommonRoomRentalRequest> GetByResidentAndCommonRoom(long residentId, long commonRoomId);

        public List<DateTime> GetReservedDates(long commonRoomId, long buildingId);

        public int UpdateStatusAndAssignedDate(long requestId, RentalRequestStatus newStatus, DateTime? assignedDate);
        public int UpdateStatusAndRange(long requestId, RentalRequestStatus newStatus, DateTime proposedStartDate, DateTime proposedEndDate);
    }
}