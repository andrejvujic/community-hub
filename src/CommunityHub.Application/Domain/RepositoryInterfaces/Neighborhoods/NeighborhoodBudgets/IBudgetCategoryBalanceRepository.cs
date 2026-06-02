using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodBudgets;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.NeighborhoodBudgets
{
    public interface IBudgetCategoryBalanceRepository
    {
        public List<BudgetCategoryBalance> GetByNeighborhoodId(long neighborhoodId);
        public void UpdateTotalAmount(long id, decimal newAmount);
    }
}
