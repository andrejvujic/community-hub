using System.Runtime.InteropServices;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.Neighborhoods;
using CommunityHub.Application.Domain.Neighborhoods.RepositoryInterfaces;
using CommunityHub.Application.Domain.Users.RepositoryInterfaces;

namespace CommunityHub.Application.Services.Neighborhoods
{
    public class NeighborhoodAccessRequestService
    {
        private readonly INeighborhoodAccessRequestRepository _neighborhoodAccessRequestRepository
            = Injector.CreateInstance<INeighborhoodAccessRequestRepository> ();
        private readonly INeighborhoodMembershipRepository _neighborhoodMembershipRepository
            = Injector.CreateInstance<INeighborhoodMembershipRepository> ();
        private readonly ICitizenRepository _citizenRepository
            = Injector.CreateInstance<ICitizenRepository> ();

        public List<NeighborhoodAccessRequest> GetByCoordinatorAndStatus(long coordinatorId, Status? status)
            => _neighborhoodAccessRequestRepository.GetByCoordinatorIdAndStatus(coordinatorId, status);

        public void Reject(NeighborhoodAccessRequest request, string comment)
        {
            request.Reject(comment);
            _neighborhoodAccessRequestRepository.Update(request);
        }

        public void Accept(NeighborhoodAccessRequest request)
        {
            request.Accept();
            _neighborhoodAccessRequestRepository.Update(request);
            var membership = new NeighborhoodMembership(
                0,
                DateTime.Now,
                request.Neighborhood
            );
            var createdMembership = _neighborhoodMembershipRepository.Create(membership);
            _citizenRepository.UpdateMembership(request.Citizen.Id, createdMembership.Id);
        }
    }
}
