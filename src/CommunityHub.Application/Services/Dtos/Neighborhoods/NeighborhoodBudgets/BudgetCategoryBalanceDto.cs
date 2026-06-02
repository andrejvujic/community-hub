namespace CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodBudgets
{
    public class BudgetCategoryBalanceDto
    {
        public long Id { get; set; }
        public NeighborhoodDto Neighborhood { get; set; }
        public BudgetCategoryDto Category { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalDonations { get; }
        public decimal TotalExpenses { get; }

        public string TotalDonationsDisplay => $"{TotalDonations:N2} RSD";
        public string TotalExpensesDisplay => $"{TotalExpenses:N2} RSD";
        public string AvailableDisplay => $"{TotalAmount:N2} RSD";

        public BudgetCategoryBalanceDto(
            long id,
            NeighborhoodDto neighborhood,
            BudgetCategoryDto category,
            decimal totalAmount,
            decimal totalDonations,
            decimal totalExpenses
        )
        {
            Id = id;
            Neighborhood = neighborhood;
            Category = category;
            TotalAmount = totalAmount;
            TotalDonations = totalDonations;
            TotalExpenses = totalExpenses;
        }
    }
}
