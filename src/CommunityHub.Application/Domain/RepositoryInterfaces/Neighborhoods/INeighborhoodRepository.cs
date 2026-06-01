using CommunityHub.Application.Domain.Neighborhoods;
using CommunityHub.Application.Helpers.Search;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods
{
    public interface INeighborhoodRepository
    {
        public List<Neighborhood> GetByCoordinatorId(long id);
        public Neighborhood Create(Neighborhood neighborhood);
        public List<Neighborhood> Search(NeighborhoodSearchParameters searchParameters);
        public void UpdateBudget(long neighborhoodId, int newBudget);
        (int CitizenCount, int NeighborhoodCount) GetStatsByCoordinatorId(long coordinatorId);
    }
}
