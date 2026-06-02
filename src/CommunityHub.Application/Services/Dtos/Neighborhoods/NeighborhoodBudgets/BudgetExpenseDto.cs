using CommunityHub.Application.Domain.Ads;
using CommunityHub.Application.Domain.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodBudgets
{
    public class BudgetExpenseDto
    {
        public long Id { get; private set; }
        public BudgetCategoryBalanceDto BudgetCategoryBalance { get; private set; }
        public UserDto Coordinator { get; private set; }
        public decimal Amount { get; private set; }
        public string Description { get; private set; }
        public DateOnly ExpenseDate { get; private set; }

        public string AmountDisplay => $"{Amount:N2} RSD";
        public string DateDisplay => ExpenseDate.ToString("dd.MM.yyyy.");

        public BudgetExpenseDto(
            long id,
            BudgetCategoryBalanceDto budgetCategoryBalance,
            UserDto coordinator,
            decimal amount,
            string description,
            DateOnly expenseDate
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