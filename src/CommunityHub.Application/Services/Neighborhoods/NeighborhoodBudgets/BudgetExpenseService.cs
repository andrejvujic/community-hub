using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.MappersDto.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.MappersDto.Users;
using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.Services.Neighborhoods.NeighborhoodBudgets
{
    public class BudgetExpenseService : BaseService<BudgetExpense, BudgetExpenseDto>
    {
        private readonly IBudgetExpenseRepository _budgetExpenseRepository
            = Injector.CreateInstance<IBudgetExpenseRepository>();

        private readonly BudgetCategoryBalanceMapper _budgetCategoryBalanceMapper = new();
        private readonly UserMapper _userMapper = new();

        public BudgetExpenseService() : base(new BudgetExpenseMapper()) { }

        public List<BudgetExpenseDto> GetByNeighborhood(long neighborhoodId)
            => MapToDto(_budgetExpenseRepository.GetByNeighborhoodId(neighborhoodId));

        public List<BudgetExpenseDto> GetByCategory(long categoryId)
            => MapToDto(_budgetExpenseRepository.GetByCategoryId(categoryId));

        public BudgetExpenseDto Create(
            BudgetCategoryBalanceDto balanceDto,
            UserDto coordinatorDto,
            decimal amount,
            string description)
        {
            if (amount > balanceDto.TotalAmount)
                throw new InvalidOperationException(
                    $"Nedovoljno sredstava u kategoriji. Raspoloživo: {balanceDto.TotalAmount:N2} RSD, " +
                    $"traženo: {amount:N2} RSD.");

            var expense = new BudgetExpense(
                _budgetCategoryBalanceMapper.MapToEntity(balanceDto),
                _userMapper.MapToEntity(coordinatorDto),
                amount,
                description,
                DateOnly.FromDateTime(DateTime.Today)
            );

            var createdExpense = _budgetExpenseRepository.Create(expense);
            return MapToDto(createdExpense);
        }
    }
}
