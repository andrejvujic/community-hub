using CommunityHub.Application.Domain.Neighborhoods;
using CommunityHub.Application.Domain.TrustRecords;
using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods
{
    public interface INeighborhoodMembershipRepository
    {
        public NeighborhoodMembership Create(NeighborhoodMembership membership);
        public List<Citizen> GetCitizensByNeighborhoodId(long neighborhoodId);
        public List<CitizenTrustRecord> GetTrustRecordsByNeighborhoodId(long neighborhoodId);
    }
}
