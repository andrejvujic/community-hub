using CommunityHub.Application.Services.Dtos.Buildings;
using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.MappersDto.Users;
using CommunityHub.Application.MappersDto.Addresses;
using CommunityHub.Application.MappersDto.Images;

namespace CommunityHub.Application.MappersDto.Buildings
{
    public class BuildingMapper : IMapper<Building, BuildingDto>
    {
        private readonly UserMapper _userMapper = new();
        private readonly AddressMapper _addressMapper = new();
        private readonly ImageMapper _imageMapper = new();
        public BuildingDto MapToDto(Building building)
        {
            if (building == null) throw new ArgumentNullException(nameof(building));

            return new BuildingDto(
                building.Id,
                building.FloorCount,
                building.ApartmentCount,
                _userMapper.MapToDto(building.Manager),
                _addressMapper.MapToDto(building.Address),
                building.DisplayAddress,
                building.Images?.Select(_imageMapper.MapToDto).ToList()
            );
        }
        public Building MapToDomain(BuildingDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var images = dto.Images?.Select(_imageMapper.MapToDomain).ToList();

            var building = new Building(
                dto.FloorCount,
                dto.ApartmentCount,
                _userMapper.MapToDomain(dto.Manager),
                _addressMapper.MapToDomain(dto.Address),
                dto.Id
            );

            return images is { Count: > 0 }
                ? building.CopyWith(images: images)
                : building;
        }
    }
}