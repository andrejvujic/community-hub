using CommunityHub.Application.Domain.Neighborhoods;
using CommunityHub.Application.MappersDto.Users;
using CommunityHub.Application.Services.Dtos.Neighborhoods;

namespace CommunityHub.Application.MappersDto.Neighborhoods
{
    public class NeighborhoodAccessRequestMapper : IMapper<NeighborhoodAccessRequest, NeighborhoodAccessRequestDto>
    {
        private readonly CitizenMapper _citizenMapper = new CitizenMapper();
        private readonly NeighborhoodMapper _neighborhoodMapper = new NeighborhoodMapper();

        public NeighborhoodAccessRequestDto MapToDto(NeighborhoodAccessRequest neighborhoodAccessRequest)
        {
            if (neighborhoodAccessRequest == null) throw new ArgumentNullException(nameof(neighborhoodAccessRequest));

            return new NeighborhoodAccessRequestDto(
                _citizenMapper.MapToDto(neighborhoodAccessRequest.Citizen),
                _neighborhoodMapper.MapToDto(neighborhoodAccessRequest.Neighborhood),
                neighborhoodAccessRequest.Status,
                neighborhoodAccessRequest.CreationDate,
                neighborhoodAccessRequest.Comment
            );
        }

        public NeighborhoodAccessRequest MapToDomain(NeighborhoodAccessRequestDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new NeighborhoodAccessRequest(
                _citizenMapper.MapToDomain(dto.Citizen),
                _neighborhoodMapper.MapToDomain(dto.Neighborhood),
                dto.Status,
                dto.CreationDate,
                dto.Comment
            );
        }
    }
}
