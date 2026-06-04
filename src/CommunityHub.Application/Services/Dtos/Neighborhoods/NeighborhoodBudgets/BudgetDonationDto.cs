using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodBudgets
{
    public class BudgetDonationDto
    {
        public long Id { get; private set; }
        public NeighborhoodDto Neighborhood { get; private set; }
        public BudgetCategoryDto BudgetCategory { get; private set; }
        public CitizenDto Citizen { get; private set; }
        public decimal Amount { get; private set; }
        public DateOnly DonationDate { get; private set; }

        public string AmountDisplay => $"{Amount:N2} RSD";
        public string DateDisplay => DonationDate.ToString("dd.MM.yyyy.");

        public BudgetDonationDto(
            long id,
            NeighborhoodDto neighborhood,
            BudgetCategoryDto budgetCategory,
            CitizenDto citizen,
            decimal amount,
            DateOnly donationDate
        )
        {
            Id = id;
            Neighborhood = neighborhood;
            BudgetCategory = budgetCategory;
            Citizen = citizen;
            Amount = amount;
            DonationDate = donationDate;
        }
    }
}
