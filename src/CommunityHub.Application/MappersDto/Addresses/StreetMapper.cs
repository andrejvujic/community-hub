using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Services.Dtos.Addresses;

namespace CommunityHub.Application.MappersDto.Addresses
{
    public class StreetMapper : IMapper<Street, StreetDto>
    {
        public StreetDto MapToDto(Street street)
        {
            if (street == null) return null!;
            return new StreetDto(street.Id, street.Name);
        }

        public Street MapToDomain(StreetDto streetDto)
        {
            if (streetDto == null) return null!;
            return new Street(streetDto.Id, streetDto.Name);
        }
    }
}
