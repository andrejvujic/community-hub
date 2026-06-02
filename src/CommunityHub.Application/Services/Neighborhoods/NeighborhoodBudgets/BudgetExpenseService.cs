using CommunityHub.Application.Database.Repositories.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.MappersDto.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodBudgets;

namespace CommunityHub.Application.Services.Neighborhoods.NeighborhoodBudgets
{
    public class BudgetExpenseService : BaseService<BudgetExpense, BudgetExpenseDto>
    {
        private readonly IBudgetExpenseRepository _budgetExpenseRepository
            = Injector.CreateInstance<IBudgetExpenseRepository>();

        public BudgetExpenseService() : base(new BudgetExpenseMapper()) { }

        public List<BudgetExpenseDto> GetByNeighborhood(long neighborhoodId)
            => MapToDto(_budgetExpenseRepository.GetByNeighborhoodId(neighborhoodId));
    }
}
