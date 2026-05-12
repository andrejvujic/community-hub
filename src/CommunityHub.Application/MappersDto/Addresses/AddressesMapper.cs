using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Services.Dtos.Addresses;

namespace CommunityHub.Application.MappersDto.Addresses
{
    public class AddressesMapper : IMapper<Address, AddressDto>
    {
        private readonly StreetMapper _streetMapper = new();
        private readonly CityMapper _cityMapper = new();
        private readonly CountryMapper _countryMapper = new();
        public AddressDto MapToDto(Address address)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));

            return new AddressDto(
                address.Id,
                _streetMapper.MapToDto(address.Street),
                address.StreetNumber,
                address.Neighborhood,
                _cityMapper.MapToDto(address.City),
                _countryMapper.MapToDto(address.Country)
            );
        }

        public Address MapToDomain(AddressDto addressDto)
        {
            if (addressDto == null) throw new ArgumentNullException(nameof(addressDto));
            return new Address(
                addressDto.Id,
                _streetMapper.MapToDomain(addressDto.Street),
                addressDto.StreetNumber,
                addressDto.Neighborhood,
                _cityMapper.MapToDomain(addressDto.City),
                _countryMapper.MapToDomain(addressDto.Country)
            );
        }
    }
}