using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodBudgets;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.NeighborhoodBudgets
{
    public interface IBudgetExpenseRepository
    {
        public List<BudgetExpense> GetByNeighborhoodId(long neighborhoodId);
        public BudgetExpense Create(BudgetExpense budgetExpense);
    }
}
