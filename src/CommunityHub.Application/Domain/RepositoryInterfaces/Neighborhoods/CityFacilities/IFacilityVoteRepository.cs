using CommunityHub.Application.Domain.Neighborhoods.CityFacilities;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.CityFacilities
{
    public interface IFacilityVoteRepository
    {
        FacilityVote? GetById(long id);
        FacilityVote? GetByFacilityIdAndCitizenId(long facilityId, long citizenId);
        List<FacilityVote> GetByNeighborhoodId(long neighborhoodId);
        public int GetVoteCountByFacilityAndNeighborhoodId(long facilityId, long neighborhoodId);
        public FacilityVote Create(FacilityVote vote);
        void DeleteByFacilityIdAndCitizenId(long facilityId, long citizenId);
        public void DeleteAllByFacilityAndNeighborhoodId(long facilityId, long neighborhoodId);
    }
}
