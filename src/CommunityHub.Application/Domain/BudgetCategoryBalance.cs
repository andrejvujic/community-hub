namespace CommunityHub.Application.Domain
{
    public class BudgetCategoryBalance
    {
        public long Id { get; private set; }
        public Neighborhood Neighborhood { get; private set; }
        public BudgetCategory Category { get; private set; }
        public decimal TotalAmount { get; private set; }

        public BudgetCategoryBalance(
            Neighborhood neighborhood,
            BudgetCategory category,
            decimal totalAmount = 0,
            long id = 0
        )
        {
            Id = id;
            Neighborhood = neighborhood;
            Category = category;
            TotalAmount = totalAmount;
        }

        public BudgetCategoryBalance CopyWith(
            long? id = null,
            Neighborhood? neighborhood = null,
            BudgetCategory? category = null,
            decimal? totalAmount = null
        )
        {
            return new BudgetCategoryBalance(
                neighborhood ?? Neighborhood,
                category ?? Category,
                totalAmount ?? TotalAmount,
                id ?? Id
            );
        }
    }
}