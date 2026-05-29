using CommunityHub.Application.Domain.Neighborhoods.CityFacilities;
using CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.CityFacilities;
using CommunityHub.Application.MappersDto.Neighborhoods.CityFacilities;
using CommunityHub.Application.Services.Dtos.Neighborhoods.CityFacilities;

namespace CommunityHub.Application.Services.Neighborhoods.CityFacilities
{
    public class FacilityVoteService : BaseService<FacilityVote, FacilityVoteDto>
    {
        private readonly IFacilityVoteRepository _voteRepository = Injector.CreateInstance<IFacilityVoteRepository>();
        private readonly FacilityVoteMapper _voteMapper = new();

        public FacilityVoteService() : base(mapper: new FacilityVoteMapper()) { }

        public int GetVoteCountByFacilityId(long facilityId)
        {
            return _voteRepository.GetVoteCountByFacilityId(facilityId);
        }

        public FacilityVoteDto? GetByFacilityIdAndCitizenId(long facilityId, long citizenId)
        {
            var votes = _voteRepository.GetByNeighborhoodId(0);
            var vote = votes.FirstOrDefault(v => v.Facility.Id == facilityId && v.Citizen.Id == citizenId);
            if (vote is null) return null;
            return _voteMapper.MapToDto(vote);
        }
    }
}
