using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodBudgets;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.NeighborhoodBudgets
{
    public interface IBudgetExpenseRepository
    {
        public List<BudgetExpense> GetByNeighborhoodId(long neighborhoodId);
        public List<BudgetExpense> GetByCategoryId(long categoryId);
        public BudgetExpense Create(BudgetExpense budgetExpense);
    }
}
