using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Services.Dtos.Addresses;

namespace CommunityHub.Application.MappersDto.Addresses
{
    public class CountryMapper : IMapper<Country, CountryDto>
    {
        public CountryDto MapToDto(Country country)
        {
            if (country == null) throw new ArgumentNullException(nameof(country));

            return new CountryDto(country.Id, country.Name);
        }
        public Country MapToDomain(CountryDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new Country(dto.Id, dto.Name);
        }
    }
}