using CommunityHub.Application.Domain.Addresses;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.Addresses
{
    public interface IAddressRepository
    {
        public List<int> GetOccupiedStreetNumbers(long? streetId, long? cityId);
        public Address Create(Address address);
    }
}