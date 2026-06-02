using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.MappersDto.Users;
using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodBudgets;

namespace CommunityHub.Application.MappersDto.Neighborhoods.NeighborhoodBudgets
{
    public class BudgetExpenseMapper : IMapper<BudgetExpense, BudgetExpenseDto>
    {
        private readonly BudgetCategoryBalanceMapper _budgetCategoryBalanceMapper = new BudgetCategoryBalanceMapper();
        private readonly UserMapper _userMapper = new UserMapper();

        public BudgetExpenseDto MapToDto(BudgetExpense budgetExpense)
        {
            if (budgetExpense == null) throw new ArgumentNullException(nameof(budgetExpense));

            return new BudgetExpenseDto(
                budgetExpense.Id,
                _budgetCategoryBalanceMapper.MapToDto(budgetExpense.BudgetCategoryBalance),
                _userMapper.MapToDto(budgetExpense.Coordinator),
                budgetExpense.Amount,
                budgetExpense.Description,
                budgetExpense.ExpenseDate
            );
        }

        public BudgetExpense MapToEntity(BudgetExpenseDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new BudgetExpense(
                _budgetCategoryBalanceMapper.MapToEntity(dto.BudgetCategoryBalance),
                _userMapper.MapToEntity(dto.Coordinator),
                dto.Amount,
                dto.Description,
                dto.ExpenseDate,
                dto.Id
            );
        }
    }
}
