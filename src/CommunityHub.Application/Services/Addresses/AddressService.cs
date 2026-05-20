using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Domain.RepositoryInterfaces.Addresses;

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