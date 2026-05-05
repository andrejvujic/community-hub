using CommunityHub.Application.Domain.Apartments;

namespace CommunityHub.Application.Domain.RepositoryInterfaces
{
    public interface IApartmentMembershipRepository
    {
        public ApartmentMembership Create(ApartmentMembership membership);
        public bool HasMembership(long userId, long apartmentId);
        public List<ApartmentMembership> GetByResident(long id);
        public List<ApartmentMembership> GetByUserAndBuilding(long userId, long buildingId);
    }
}