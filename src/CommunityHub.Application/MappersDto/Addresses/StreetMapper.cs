using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Services.Dtos.Addresses;

namespace CommunityHub.Application.MappersDto.Addresses
{
    public class StreetMapper : IMapper<Street, StreetDto>
    {
        public StreetDto MapToDto(Street street)
        {
            if (street == null) throw new ArgumentNullException(nameof(street));

            return new StreetDto(street.Id, street.Name);
        }
        public Street MapToEntity(StreetDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new Street(dto.Id, dto.Name);
        }
    }
} 