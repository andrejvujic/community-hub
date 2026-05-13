using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.Neighborhoods;
using CommunityHub.Application.Domain.Neighborhoods.RepositoryInterfaces;

namespace CommunityHub.Application.Services.Neighborhoods
{
    public class NeighborhoodAccessRequestService
    {
        private readonly INeighborhoodAccessRequestRepository _neighborhoodAccessRequestRepository
            = Injector.CreateInstance<INeighborhoodAccessRequestRepository> ();

        public List<NeighborhoodAccessRequest> GetByCoordinatorAndStatus(long coordinatorId, Status? status)
            => _neighborhoodAccessRequestRepository.GetByCoordinatorIdAndStatus(coordinatorId, status);
    }
}
