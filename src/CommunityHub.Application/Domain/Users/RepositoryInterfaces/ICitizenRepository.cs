using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.Domain.Users.RepositoryInterfaces
{
    public interface ICitizenRepository
    {
        public List<Citizen> GetByNeighborhoodId(long neighborhoodId);
        public Citizen? GetById(long id);
        public void UpdateMembership(long citizenId, long? membershipId);
    }
}
