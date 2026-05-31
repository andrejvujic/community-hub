using CommunityHub.Application.Domain.Neighborhoods.CityFacilities;
using CommunityHub.Application.MappersDto.Users;
using CommunityHub.Application.Services.Dtos.Neighborhoods.CityFacilities;

namespace CommunityHub.Application.MappersDto.Neighborhoods.CityFacilities
{
    public class FacilityVoteMapper : IMapper<FacilityVote, FacilityVoteDto>
    {
        private readonly CityFacilityMapper _facilityMapper = new();
        private readonly CitizenMapper _citizenMapper = new();

        public FacilityVoteDto MapToDto(FacilityVote facilityVote)
        {
            if (facilityVote == null) throw new ArgumentNullException(nameof(facilityVote));

            return new FacilityVoteDto(
                facilityVote.Id,
                _facilityMapper.MapToDto(facilityVote.Facility),
                _citizenMapper.MapToDto(facilityVote.Citizen)
            );
        }

        public FacilityVote MapToEntity(FacilityVoteDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new FacilityVote(
                _facilityMapper.MapToEntity(dto.Facility),
                _citizenMapper.MapToEntity(dto.Citizen),
                dto.Id
            );
        }
    }
}
