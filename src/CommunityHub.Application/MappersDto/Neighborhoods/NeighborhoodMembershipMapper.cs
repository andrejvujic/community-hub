using CommunityHub.Application.Domain.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Neighborhoods;

namespace CommunityHub.Application.MappersDto.Neighborhoods
{
    public class NeighborhoodMembershipMapper : IMapper<NeighborhoodMembership, NeighborhoodMembershipDto>
    {
        private readonly NeighborhoodMapper _neighborhoodMapper = new NeighborhoodMapper();

        public NeighborhoodMembershipDto MapToDto(NeighborhoodMembership neighborhoodMembership)
        {
            if (neighborhoodMembership == null) throw new ArgumentNullException(nameof(neighborhoodMembership));

            return new NeighborhoodMembershipDto(
                neighborhoodMembership.Id,
                neighborhoodMembership.AcceptanceDate,
                _neighborhoodMapper.MapToDto(neighborhoodMembership.Neighborhood)
            );
        }

        public NeighborhoodMembership MapToDomain(NeighborhoodMembershipDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new NeighborhoodMembership(
                dto.Id,
                dto.AcceptanceDate,
                _neighborhoodMapper.MapToDomain(dto.Neighborhood)
            );
        }
    }
}
