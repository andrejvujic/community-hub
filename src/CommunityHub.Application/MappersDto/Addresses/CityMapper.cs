using CommunityHub.Application.Services.Dtos.Addresses;
using CommunityHub.Application.Domain.Addresses;

namespace CommunityHub.Application.MappersDto.Addresses
{
    public class CityMapper : IMapper<City, CityDto>
    {
        public CityDto MapToDto(City city)
        {
            if (city == null) throw new ArgumentNullException(nameof(city));

            return new CityDto(city.Id, city.Name);
        }
        public City MapToEntity(CityDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new City(dto.Id, dto.Name);
        }
    }
}