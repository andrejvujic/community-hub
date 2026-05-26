using CommunityHub.Application.Domain.Apartments;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.Apartments
{
    public interface IApartmentMembershipRepository
    {
        public ApartmentMembership Create(ApartmentMembership membership);

        public List<ApartmentMembership> GetByResident(long id);
        public List<ApartmentMembership> GetByApartment(long id);
        public List<ApartmentMembership> GetByUserAndBuilding(long userId, long buildingId);
        public List<long> GetApartmentIdsByResidentAndBuilding(long residentId, long buildingId);

        public bool Exists(long apartmentId);
        public bool Exists(long residentId, long apartmentId);
    }
}
