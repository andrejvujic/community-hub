using CommunityHub.Application.Services.Dtos.Apartments;
using CommunityHub.Application.Domain.Apartments;
using CommunityHub.Application.MappersDto.Buildings;

namespace CommunityHub.Application.MappersDto.Apartments
{
    public class ApartmentMapper : IMapper<Apartment, ApartmentDto>
    {
        private readonly BuildingMapper _buildingMapper = new();
        public ApartmentDto MapToDto(Apartment apartment)
        {
            if (apartment == null) throw new ArgumentNullException(nameof(apartment));

            return new ApartmentDto(
                apartment.Id,
                apartment.Number,
                apartment.FloorNumber,
                _buildingMapper.MapToDto(apartment.Building)
            );
        }
        public Apartment MapToDomain(ApartmentDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new Apartment(
                dto.Id,
                dto.Number,
                dto.FloorNumber,
                _buildingMapper.MapToDomain(dto.Building)
            );
        }
    }
}