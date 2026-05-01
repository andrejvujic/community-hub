using System.Data;
using CommunityHub.Application.Domain.NeighborhoodBudget;

namespace CommunityHub.Application.Database.Mapping.EntityMappers.NeighborhoodBudget
{
    public class BudgetExpenseMapper : EntityMapper<BudgetExpense>
    {
        public override BudgetExpense? MapToEntity(IDataReader reader)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<string, object?> MapToQueryParameters(BudgetExpense entity)
        {
            throw new NotImplementedException();
        }
    }
}
