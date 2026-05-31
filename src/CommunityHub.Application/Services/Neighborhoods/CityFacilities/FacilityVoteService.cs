using CommunityHub.Application.Domain.Neighborhoods.CityFacilities;
using CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.CityFacilities;
using CommunityHub.Application.Domain.RepositoryInterfaces.Users;
using CommunityHub.Application.MappersDto.Neighborhoods.CityFacilities;
using CommunityHub.Application.Services.Dtos.Neighborhoods.CityFacilities;

namespace CommunityHub.Application.Services.Neighborhoods.CityFacilities
{
    public class FacilityVoteService : BaseService<FacilityVote, FacilityVoteDto>
    {
        private readonly IFacilityVoteRepository _voteRepository = Injector.CreateInstance<IFacilityVoteRepository>();
        private readonly ICityFacilityRepository _facilityRepository = Injector.CreateInstance<ICityFacilityRepository>();
        private readonly ICitizenRepository _citizenRepository = Injector.CreateInstance<ICitizenRepository>();
        private readonly FacilityVoteMapper _voteMapper = new();

        public FacilityVoteService() : base(mapper: new FacilityVoteMapper()) { }

        public int GetVoteCountByFacilityId(long facilityId)
        {
            return _voteRepository.GetVoteCountByFacilityId(facilityId);
        }

        public FacilityVoteDto? GetByFacilityIdAndCitizenId(long facilityId, long citizenId)
        {
            var vote = _voteRepository.GetByFacilityIdAndCitizenId(facilityId, citizenId);
            if (vote is null) return null;
            return _voteMapper.MapToDto(vote);
        }

        public void DeleteByFacilityIdAndCitizenId(long facilityId, long citizenId)
        {
            _voteRepository.DeleteByFacilityIdAndCitizenId(facilityId, citizenId);
        }

        public void Create(long facilityId, long citizenId)
        {
            var facility = _facilityRepository.GetById(facilityId);
            var citizen = _citizenRepository.GetById(citizenId);

            if (facility != null && citizen != null)
            {
                var vote = new FacilityVote(facility, citizen);
                _voteRepository.Create(vote);
            }
        }

        public void DeleteAllByFacilityId(long facilityId)
            => _voteRepository.DeleteAllByFacilityId(facilityId);
    }
}
