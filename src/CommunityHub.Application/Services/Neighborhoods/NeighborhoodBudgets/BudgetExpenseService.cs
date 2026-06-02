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

        public BudgetExpenseDto Create(
            BudgetCategoryBalanceDto balance,
            UserDto coordinator,
            decimal amount,
            string description)
        {
            if (amount > balance.TotalAmount)
                throw new InvalidOperationException(
                    $"Nedovoljno sredstava u kategoriji. Raspoloživo: {balance.TotalAmount:N2} RSD, " +
                    $"traženo: {amount:N2} RSD.");

            var expense = new BudgetExpense(
                _budgetCategoryBalanceMapper.MapToEntity(balance),
                _userMapper.MapToEntity(coordinator),
                amount,
                description,
                DateOnly.FromDateTime(DateTime.Today)
            );

            var createdExpense = _budgetExpenseRepository.Create(expense);
            return MapToDto(createdExpense);
        }
    }
}
