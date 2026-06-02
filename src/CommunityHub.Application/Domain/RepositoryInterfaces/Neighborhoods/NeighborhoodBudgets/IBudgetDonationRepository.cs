using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodBudgets;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.NeighborhoodBudgets
{
    public interface IBudgetDonationRepository
    {
        public List<BudgetDonation> GetByNeighborhoodId(long neighborhoodId);
        public List<BudgetDonation> GetByNeighborhoodIdAndCategoryId(long neighborhoodId, long categoryId);
        public BudgetDonation Create(BudgetDonation budgetDonation);
    }
}
