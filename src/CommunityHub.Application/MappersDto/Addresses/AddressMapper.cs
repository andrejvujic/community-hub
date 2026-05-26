using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Services.Dtos.Addresses;

namespace CommunityHub.Application.MappersDto.Addresses
{
    public class AddressMapper : IMapper<Address, AddressDto>
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
        public Address MapToEntity(AddressDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new Address(
                dto.Id,
                _streetMapper.MapToEntity(dto.Street),
                dto.StreetNumber,
                dto.Neighborhood,
                _cityMapper.MapToEntity(dto.City),
                _countryMapper.MapToEntity(dto.Country)
            );
        }
    }
}