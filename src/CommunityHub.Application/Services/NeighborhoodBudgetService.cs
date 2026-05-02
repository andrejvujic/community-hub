using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Database.Repositories.NeighborhoodBudget;

namespace CommunityHub.Application.Services
{
    public class NeighborhoodBudgetService
    {
        private readonly BudgetCategoryBalanceRepository _balanceRepository;
        private readonly BudgetExpenseRepository _expenseRepository;
        //private readonly BudgetDonationRepository _donationRepository;
        private readonly NeighborhoodRepository _neighborhoodRepository;

        public NeighborhoodBudgetService(
            BudgetCategoryBalanceRepository balanceRepository,
            BudgetExpenseRepository expenseRepository,
            //BudgetDonationRepository donationRepository,
            NeighborhoodRepository neighborhoodRepository)
        {
            _balanceRepository = balanceRepository;
            _expenseRepository = expenseRepository;
            //_donationRepository = donationRepository;
            _neighborhoodRepository = neighborhoodRepository;
        }
    }
}
