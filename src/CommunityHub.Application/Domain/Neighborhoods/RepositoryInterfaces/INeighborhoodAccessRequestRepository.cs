using CommunityHub.Application.Helpers.Sorting;

namespace CommunityHub.Application.Domain.Neighborhoods.RepositoryInterfaces
{
    public interface INeighborhoodAccessRequestRepository
    {
        public List<NeighborhoodAccessRequest> GetWaitingByNeighborhoodId(long neighborhoodId);
        public List<NeighborhoodAccessRequest> GetByCitizenId(long citizenId, RequestOrder order = RequestOrder.Unspecified, bool filterByStatus = false);
        public List<NeighborhoodAccessRequest> GetWaitingByCitizenId(long citizenId);
        public List<NeighborhoodAccessRequest> GetByCoordinatorId(long coordinatorId);
        public List<NeighborhoodAccessRequest> GetByCoordinatorIdAndStatus(long coordinatorId, Status? status);
        public void Update(NeighborhoodAccessRequest request);
        public bool Create(NeighborhoodAccessRequest request);
        public void Delete(long citizenId, long neighborhoodId);
        public bool HasPendingRequest(long userId, long neighborhoodId);
    }
}
