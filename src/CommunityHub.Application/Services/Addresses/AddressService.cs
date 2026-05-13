using CommunityHub.Application.Domain.Addresses.RepositoryInterfaces;
using CommunityHub.Application.Domain.Addresses;

namespace CommunityHub.Application.Services.Addresses
{
    public class AddressService
    {
        private readonly IAddressRepository _addressRepository = Injector.CreateInstance<IAddressRepository>();

        public List<int> GetOccupiedStreetNumbers(long streetId, long cityId)
            => _addressRepository.GetOccupiedStreetNumbers(streetId, cityId);

        public Address Create(Address address) 
            => _addressRepository.Create(address);
    }
}