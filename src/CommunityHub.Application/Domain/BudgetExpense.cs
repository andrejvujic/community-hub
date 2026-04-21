namespace CommunityHub.Application.Domain
{
    public class BudgetExpense
    {
        public long Id { get; private set; }
        public BudgetCategoryBalance BudgetCategoryBalance { get; private set; }
        public User Coordinator { get; private set; }
        public decimal Amount { get; private set; }
        public string Description { get; private set; }
        public DateOnly ExpenseDate { get; private set; }

        public BudgetExpense(
            BudgetCategoryBalance budgetCategoryBalance,
            User coordinator,
            decimal amount,
            string description,
            DateOnly expenseDate,
            long id = 0
        )
        {
            Id = id;
            BudgetCategoryBalance = budgetCategoryBalance;
            Coordinator = coordinator;
            Amount = amount;
            Description = description;
            ExpenseDate = expenseDate;
        }
    }
}