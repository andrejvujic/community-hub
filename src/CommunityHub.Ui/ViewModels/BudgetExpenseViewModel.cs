using CommunityHub.Application.Domain.NeighborhoodBudget;

namespace CommunityHub.Ui.ViewModels
{
    public class BudgetExpenseViewModel
    {
        public BudgetExpense Expense { get; }

        public string Description => Expense.Description;
        public string AmountDisplay => $"{Expense.Amount:N2} RSD";
        public string DateDisplay => Expense.ExpenseDate.ToString("dd.MM.yyyy.");
        
        public BudgetExpenseViewModel(BudgetExpense expense)
        {
            Expense = expense;
        }
    }
}
