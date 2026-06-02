using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodBudgets;

namespace CommunityHub.Application.MappersDto.Neighborhoods.NeighborhoodBudgets
{
    public class BudgetCategoryMapper : IMapper<BudgetCategory, BudgetCategoryDto>
    {
        public BudgetCategoryDto MapToDto(BudgetCategory budgetCategory)
        {
            if (budgetCategory == null) throw new ArgumentNullException(nameof(budgetCategory));

            return new BudgetCategoryDto(
                budgetCategory.Id,
                budgetCategory.Name
            );
        }

        public BudgetCategory MapToEntity(BudgetCategoryDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new BudgetCategory(
                dto.Id,
                dto.Name
            );
        }
    }
}