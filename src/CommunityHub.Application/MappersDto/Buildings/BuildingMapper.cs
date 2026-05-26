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
            if (building == null) throw new ArgumentNullException("Can't convert null reference to BuildingDto.");

            return new BuildingDto(
                id: building.Id,
                floorCount: building.FloorCount,
                apartmentCount: building.ApartmentCount,
                manager: _userMapper.MapToDto(building.Manager),
                address: _addressMapper.MapToDto(building.Address),
                displayAddress: building.DisplayAddress,
                building.Images?.Select(_imageMapper.MapToDto).ToList()
            );
        }

        public Building MapToEntity(BuildingDto dto)
        {
            if (dto == null) throw new ArgumentNullException("Can't convert null reference to Building.");

            var images = dto.Images?.Select(_imageMapper.MapToEntity).ToList();

            var building = new Building(
                id: dto.Id,
                floorCount: dto.FloorCount,
                apartmentCount: dto.ApartmentCount,
                manager: _userMapper.MapToEntity(dto.Manager),
                address: _addressMapper.MapToEntity(dto.Address)
            );

            return images is not null && images.Count > 0
                ? building.CopyWith(images: images)
                : building;
        }
    }
}