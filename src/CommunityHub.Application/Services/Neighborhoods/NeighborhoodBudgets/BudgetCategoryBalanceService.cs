using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.MappersDto.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodBudgets;

namespace CommunityHub.Application.Services.Neighborhoods.NeighborhoodBudgets
{
    public class BudgetCategoryBalanceService : BaseService<BudgetCategoryBalance, BudgetCategoryBalanceDto>
    {
        private readonly IBudgetCategoryBalanceRepository _budgetCategoryBalanceRepository
            = Injector.CreateInstance<IBudgetCategoryBalanceRepository>();

        public BudgetCategoryBalanceService() : base(new BudgetCategoryBalanceMapper()) { }

        public List<BudgetCategoryBalanceDto> GetByNeighborhood(long neighborhoodId)
            => MapToDto(_budgetCategoryBalanceRepository.GetByNeighborhoodId(neighborhoodId));

        public List<BudgetCategoryBalanceDto> GetByNeighborhood(long neighborhoodId, List<BudgetExpenseDto> budgetExpenseDtos)
        {
            var budgetCategoryBalanceDtos = MapToDto(_budgetCategoryBalanceRepository.GetByNeighborhoodId(neighborhoodId));

            foreach (var dto in budgetCategoryBalanceDtos)
            {
                dto.TotalExpenses = budgetExpenseDtos
                    .Where(e => e.BudgetCategoryBalance.Id == dto.Id)
                    .Sum(e => e.Amount);
            }

            return budgetCategoryBalanceDtos;
        }

        public decimal UpdateTotalAmount(
            BudgetCategoryBalanceDto balanceDto,
            decimal amountChange)
        {
            var balance = MapToEntity(balanceDto);
            var newCategoryAmount = balance.TotalAmount + amountChange;
            _budgetCategoryBalanceRepository.UpdateTotalAmount(balance.Id, newCategoryAmount);

            return balance.Neighborhood.Budget + amountChange;
        }
    }
}
