using CommunityHub.Application.Domain.Neighborhoods.RepositoryInterfaces;
using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.Services.Neighborhoods
{
    public class NeighborhoodMembershipService
    {
        private readonly INeighborhoodMembershipRepository _neighborhoodMembershipRepository
            = Injector.CreateInstance<INeighborhoodMembershipRepository>();

        public List<Citizen> GetCitizensByNeighborhood(long neighborhoodId)
            => _neighborhoodMembershipRepository.GetCitizensByNeighborhoodId(neighborhoodId);
        
    }
}
