using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Database.Repositories.Neighborhoods;
using CommunityHub.Application.Database.Repositories.Neighborhoods.NeighborhoodBudget;
using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodBudget;
using CommunityHub.Application.Domain.Users;

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

        public List<BudgetCategoryBalance> GetBalancesForNeighborhood(long neighborhoodId)
            => _balanceRepository.GetByNeighborhoodId(neighborhoodId);

        public List<BudgetExpense> GetExpensesForNeighborhood(long neighborhoodId)
            => _expenseRepository.GetByNeighborhoodId(neighborhoodId);

        public BudgetExpense CreateExpense(
            BudgetCategoryBalance balance,
            User coordinator,
            decimal amount,
            string description)
        {
            if (amount > balance.TotalAmount)
                throw new InvalidOperationException(
                    $"Nedovoljno sredstava u kategoriji. Raspoloživo: {balance.TotalAmount:N2} RSD, " +
                    $"traženo: {amount:N2} RSD.");

            var expense = new BudgetExpense(
                balance,
                coordinator,
                amount,
                description,
                DateOnly.FromDateTime(DateTime.Today)
            );

            var createdExpense = _expenseRepository.Create(expense);
            UpdateBalanceAndNeighborhoodBudget(balance, -amount);
            return createdExpense;
        }

        private void UpdateBalanceAndNeighborhoodBudget(
            BudgetCategoryBalance balance,
            decimal amountChange)
        {
            var newCategoryAmount = balance.TotalAmount + amountChange;
            _balanceRepository.UpdateTotalAmount(balance.Id, newCategoryAmount);

            var newNeighborhoodBudget = (int)(balance.Neighborhood.Budget + amountChange);
            _neighborhoodRepository.UpdateBudget(balance.Neighborhood.Id, newNeighborhoodBudget);
        }
    }
}
