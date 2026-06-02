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
    }
}
