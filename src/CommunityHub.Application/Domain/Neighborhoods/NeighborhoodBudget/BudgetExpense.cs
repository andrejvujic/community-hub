using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.Domain.Neighborhoods.NeighborhoodBudget
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

        public BudgetExpense CopyWith(
            long? id = null,
            BudgetCategoryBalance? budgetCategoryBalance = null,
            User? coordinator = null,
            decimal? amount = null,
            string? description = null,
            DateOnly? expenseDate = null
        )
        {
            return new BudgetExpense(
                budgetCategoryBalance ?? BudgetCategoryBalance,
                coordinator ?? Coordinator,
                amount ?? Amount,
                description ?? Description,
                expenseDate ?? ExpenseDate,
                id ?? Id
            );
        }
    }
}