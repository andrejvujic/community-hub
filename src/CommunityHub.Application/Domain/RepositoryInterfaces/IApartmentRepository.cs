using CommunityHub.Application.Domain.Apartments;

namespace CommunityHub.Application.Domain.RepositoryInterfaces
{
    public interface IApartmentRepository
    {
        public Apartment Create(Apartment apartment);
        public Apartment? Get(long id);
        public long? FindIdByBuildingAndNumber(long buildingId, int apartmentNumber);
    }
}