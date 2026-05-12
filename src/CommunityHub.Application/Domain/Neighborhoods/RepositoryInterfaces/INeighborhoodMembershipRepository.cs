using CommunityHub.Application.Domain.TrustRecords;
using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.Domain.Neighborhoods.RepositoryInterfaces
{
    public interface INeighborhoodMembershipRepository
    {
        public NeighborhoodMembership Create(NeighborhoodMembership membership);
        public List<Citizen> GetCitizensByNeighborhoodId(long neighborhoodId);
        public List<CitizenTrustRecord> GetTrustRecordsByNeighborhoodId(long neighborhoodId);
    }
}
