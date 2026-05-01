using System.Data;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.NeighborhoodBudget;

namespace CommunityHub.Application.Database.Mapping.EntityMappers.NeighborhoodBudget
{
    public class BudgetCategoryBalanceMapper : EntityMapper<BudgetCategoryBalance>
    {
        public override BudgetCategoryBalance? MapToEntity(IDataReader reader)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<string, object?> MapToQueryParameters(BudgetCategoryBalance entity)
        {
            throw new NotImplementedException();
        }
    }
}
