using CommunityHub.Application.Helpers.Search;

namespace CommunityHub.Application.Domain.Neighborhoods.RepositoryInterfaces
{
    public interface INeighborhoodRepository
    {
        public List<Neighborhood> GetByCoordinatorId(long id);
        public Neighborhood Create(Neighborhood neighborhood);
        public List<Neighborhood> Search(NeighborhoodSearchParameters searchParameters);
        public void UpdateBudget(long neighborhoodId, int newBudget);
    }
}
