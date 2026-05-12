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
        public Country MapToDomain(CountryDto countryDto)
        {
            if (countryDto == null) throw new ArgumentNullException(nameof(countryDto));
            return new Country(countryDto.Id, countryDto.Name);
        }
    }
}