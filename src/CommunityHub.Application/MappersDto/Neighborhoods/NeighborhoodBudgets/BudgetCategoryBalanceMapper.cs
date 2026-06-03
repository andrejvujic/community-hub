using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodBudgets;

namespace CommunityHub.Application.MappersDto.Neighborhoods.NeighborhoodBudgets
{
    public class BudgetCategoryBalanceMapper : IMapper<BudgetCategoryBalance, BudgetCategoryBalanceDto>
    {
        private readonly NeighborhoodMapper _neighborhoodMapper = new NeighborhoodMapper();
        private readonly BudgetCategoryMapper _budgetCategoryMapper = new BudgetCategoryMapper();

        public BudgetCategoryBalanceDto MapToDto(BudgetCategoryBalance budgetCategoryBalance)
        {
            if (budgetCategoryBalance == null) throw new ArgumentNullException(nameof(budgetCategoryBalance));

            return new BudgetCategoryBalanceDto(
                budgetCategoryBalance.Id,
                _neighborhoodMapper.MapToDto(budgetCategoryBalance.Neighborhood),
                _budgetCategoryMapper.MapToDto(budgetCategoryBalance.Category),
                budgetCategoryBalance.TotalAmount,
                0,
                0
            );
        }

        public BudgetCategoryBalance MapToEntity(BudgetCategoryBalanceDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new BudgetCategoryBalance(
                _neighborhoodMapper.MapToEntity(dto.Neighborhood),
                _budgetCategoryMapper.MapToEntity(dto.Category),
                dto.TotalAmount,
                dto.Id
            );
        }
    }
}