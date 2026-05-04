
namespace CommunityHub.Application.Domain.RepositoryInterfaces
{
    public interface IAddressRepository
    {
        public List<int> GetOccupiedStreetNumbers(long? streetId, long? cityId);
        public Address Create(Address address);
    }
}