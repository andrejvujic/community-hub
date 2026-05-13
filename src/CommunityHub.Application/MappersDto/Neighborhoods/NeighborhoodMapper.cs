using CommunityHub.Application.Domain.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.MappersDto.Addresses;
using CommunityHub.Application.MappersDto.Images;
using CommunityHub.Application.MappersDto.Users;

namespace CommunityHub.Application.MappersDto.Neighborhoods
{
    public class NeighborhoodMapper : IMapper<Neighborhood, NeighborhoodDto>
    {
        private readonly CityMapper _cityMapper = new();
        private readonly CountryMapper _countryMapper = new();
        private readonly UserMapper _userMapper = new();
        private readonly StreetSectionMapper _streetSectionMapper = new();
        private readonly ImageMapper _imageMapper = new();

        public NeighborhoodDto MapToDto(Neighborhood neighborhood)
        {
            if (neighborhood == null) throw new ArgumentNullException(nameof(neighborhood));
            
            return new NeighborhoodDto(
                neighborhood.Id,
                neighborhood.Name,
                neighborhood.Description,
                neighborhood.Budget,
                _cityMapper.MapToDto(neighborhood.City),
                _countryMapper.MapToDto(neighborhood.Country),
                _userMapper.MapToDto(neighborhood.Coordinator),
                neighborhood.StreetSections?.Select(_streetSectionMapper.MapToDto).ToList(),
                neighborhood.Images?.Select(_imageMapper.MapToDto).ToList()
            );
        }

        public Neighborhood MapToDomain(NeighborhoodDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new Neighborhood(
                dto.Name,
                dto.Description,
                dto.Budget,
                _cityMapper.MapToDomain(dto.City),
                _countryMapper.MapToDomain(dto.Country),
                _userMapper.MapToDomain(dto.Coordinator),
                dto.Id,
                dto.StreetSections?.Select(_streetSectionMapper.MapToDomain).ToList(),
                dto.Images?.Select(_imageMapper.MapToDomain).ToList()
            );
        }
    }
}