using CommunityHub.Application.Domain.Buildings.CommonRooms.RentalRequests;

namespace CommunityHub.Application.Domain.RepositoryInterfaces
{
    public interface ICommonRoomRentalRequestRepository
    {
        public void Create(CommonRoomRentalRequest request);
        public void Delete(long id);
        public void Update(CommonRoomRentalRequest request);

        public List<CommonRoomRentalRequest> GetAllByBuildingId(long buildingId);
        public List<CommonRoomRentalRequest> GetByResidentAndCommonRoom(long residentId, long commonRoomId);

        public List<DateTime> GetReservedDates(long commonRoomId, long buildingId);

        // Koristiti Update(CommonRoomRentalRequest request) - definisano iznad.
        public int UpdateStatusAndAssignedDate(long requestId, RentalRequestStatus newStatus, DateTime? assignedDate);
        public int UpdateStatusAndRange(long requestId, RentalRequestStatus newStatus, DateTime proposedStartDate, DateTime proposedEndDate);

        public bool HasPendingRequest(long userId, long commonRoomId);
    }
}