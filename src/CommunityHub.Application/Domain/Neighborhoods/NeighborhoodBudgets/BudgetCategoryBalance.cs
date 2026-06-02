namespace CommunityHub.Application.Domain.Neighborhoods.NeighborhoodBudgets
{
    public class BudgetCategoryBalance
    {
        public long Id { get; private set; }
        public Neighborhood Neighborhood { get; private set; }
        public BudgetCategory Category { get; private set; }
        public decimal TotalAmount { get; private set; }
        public decimal TotalDonations { get; }
        public decimal TotalExpenses { get; }

        public BudgetCategoryBalance(
            Neighborhood neighborhood,
            BudgetCategory category,
            decimal totalAmount = 0,
            decimal totalDonations = 0,
            decimal totalExpenses = 0,
            long id = 0
        )
        {
            Id = id;
            Neighborhood = neighborhood;
            Category = category;
            TotalAmount = totalAmount;
            TotalDonations = totalDonations;
            TotalExpenses = totalExpenses;
        }

        public BudgetCategoryBalance CopyWith(
            long? id = null,
            Neighborhood? neighborhood = null,
            BudgetCategory? category = null,
            decimal? totalAmount = null,
            decimal? totalDonations = null,
            decimal? totalExpenses = null
        )
        {
            return new BudgetCategoryBalance(
                neighborhood ?? Neighborhood,
                category ?? Category,
                totalAmount ?? TotalAmount,
                totalDonations ?? TotalDonations,
                totalExpenses ?? TotalExpenses,
                id ?? Id
            );
        }
    }
}