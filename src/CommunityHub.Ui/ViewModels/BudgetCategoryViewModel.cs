using CommunityHub.Application.Domain.NeighborhoodBudget;

namespace CommunityHub.Ui.ViewModels
{
    public class BudgetCategoryViewModel
    {
        public BudgetCategoryBalance Balance { get; }
        public decimal TotalDonations { get; }
        public decimal TotalExpenses { get; }
        public BudgetCategory Category => Balance.Category;

        public string TotalDonationsDisplay => $"{TotalDonations:N2} RSD";
        public string TotalExpensesDisplay => $"{TotalExpenses:N2} RSD";
        public string AvailableDisplay => $"{Balance.TotalAmount:N2} RSD";

        public BudgetCategoryViewModel(
            BudgetCategoryBalance balance,
            decimal totalDonations,
            decimal totalExpenses)
        {
            Balance = balance;
            TotalDonations = totalDonations;
            TotalExpenses = totalExpenses;
        }
    }
}
