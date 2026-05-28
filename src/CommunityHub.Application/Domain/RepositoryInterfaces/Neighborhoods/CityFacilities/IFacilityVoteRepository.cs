using CommunityHub.Application.Domain.Neighborhoods.CityFacilities;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.CityFacilities
{
    public interface IFacilityVoteRepository
    {
        FacilityVote? GetById(long id);
        List<FacilityVote> GetByNeighborhoodId(long neighborhoodId);
        int GetVoteCountByFacilityId(long facilityId);
        FacilityVote Create(FacilityVote vote);
        void Delete(long id);
    }
}
