using CommunityHub.Application.Domain.Neighborhoods.CityFacilities;
using CommunityHub.Application.MappersDto.Addresses;
using CommunityHub.Application.Services.Dtos.Neighborhoods.CityFacilities;

namespace CommunityHub.Application.MappersDto.Neighborhoods.CityFacilities
{
    public class CityFacilityMapper : IMapper<CityFacility, CityFacilityDto>
    {
        private readonly CityMapper _cityMapper = new();
        private readonly CountryMapper _countryMapper = new();

        public CityFacilityDto MapToDto(CityFacility cityFacility)
        {
            if (cityFacility == null) throw new ArgumentNullException(nameof(cityFacility));

            return new CityFacilityDto(
                cityFacility.Id,
                cityFacility.Name,
                _cityMapper.MapToDto(cityFacility.City),
                _countryMapper.MapToDto(cityFacility.Country)
            );
        }

        public CityFacility MapToEntity(CityFacilityDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new CityFacility(
                dto.Name,
                _cityMapper.MapToEntity(dto.City),
                _countryMapper.MapToEntity(dto.Country),
                dto.Id
            );
        }
    }
}
