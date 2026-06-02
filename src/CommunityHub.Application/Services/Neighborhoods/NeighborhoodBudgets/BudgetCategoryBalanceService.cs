using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.MappersDto.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodBudgets;

namespace CommunityHub.Application.Services.Neighborhoods.NeighborhoodBudgets
{
    public class BudgetCategoryBalanceService : BaseService<BudgetCategoryBalance, BudgetCategoryBalanceDto>
    {
        public BudgetCategoryBalanceService() : base(new BudgetCategoryBalanceMapper()) { }
    }
}
