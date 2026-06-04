using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.Domain.Neighborhoods.NeighborhoodBudgets
{
    public class BudgetDonation
    {
        public long Id { get; private set; }
        public Neighborhood Neighborhood { get; private set; }
        public BudgetCategory BudgetCategory { get; private set; }
        public Citizen Citizen { get; private set; }
        public decimal Amount { get; private set; }
        public DateOnly DonationDate { get; private set; }

        public BudgetDonation(
            Neighborhood neighborhood,
            BudgetCategory budgetCategory,
            Citizen citizen,
            decimal amount,
            DateOnly donationDate,
            long id = 0
        )
        {
            ValidateInputs(citizen, neighborhood, amount);

            Id = id;
            Neighborhood = neighborhood;
            BudgetCategory = budgetCategory;
            Citizen = citizen;
            Amount = amount;
            DonationDate = donationDate;
        }

        private static void ValidateInputs(Citizen citizen, Neighborhood neighborhood, decimal amount)
        {
            if (amount <= 0) throw new InvalidOperationException("Iznos donacije mora biti veći od 0.");
            if (citizen.Membership?.Neighborhood?.Id != neighborhood.Id) throw new ArgumentException("Citizen must be a member of the neighborhood to donate to it.");
        }

        public BudgetDonation CopyWith(
            long? id = null,
            Neighborhood? neighborhood = null,
            BudgetCategory? budgetCategory = null,
            Citizen? citizen = null,
            decimal? amount = null,
            DateOnly? donationDate = null
        )
        {
            return new BudgetDonation(
                neighborhood ?? Neighborhood,
                budgetCategory ?? BudgetCategory,
                citizen ?? Citizen,
                amount ?? Amount,
                donationDate ?? DonationDate,
                id ?? Id
            );
        }
    }
}
