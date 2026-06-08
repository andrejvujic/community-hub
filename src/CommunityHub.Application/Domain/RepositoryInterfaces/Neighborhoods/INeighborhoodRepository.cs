using CommunityHub.Application.Domain.Neighborhoods;
using CommunityHub.Application.Helpers.Search;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods
{
    public interface INeighborhoodRepository
    {
        public List<Neighborhood> GetByCoordinatorId(long id);
        public Neighborhood Create(Neighborhood neighborhood);
        public List<Neighborhood> Search(NeighborhoodSearchParameters searchParameters);
        public void UpdateBudget(long neighborhoodId, decimal newBudget);
        public int GetNeighborhoodCountsByCoordinatorId(long coordinatorId);
    }
}
