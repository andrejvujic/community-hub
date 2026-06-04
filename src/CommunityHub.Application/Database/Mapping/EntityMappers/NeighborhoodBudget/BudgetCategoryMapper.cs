using System.Data;
using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodBudgets;


namespace CommunityHub.Application.Database.Mapping.EntityMappers.NeighborhoodBudget
{
    public class BudgetCategoryMapper : EntityMapper<BudgetCategory>
    {
        public override BudgetCategory MapToEntity(IDataReader reader)
        {
            long id = reader.GetInt64(reader.GetOrdinal("budget_category_id"));
            string name = reader.GetString(reader.GetOrdinal("budget_category_name"));
            return new BudgetCategory(id, name);
        }

        public override Dictionary<string, object?> MapToQueryParameters(BudgetCategory entity)
        {
            return new Dictionary<string, object?>
            {
                { "budget_category_name", entity.Name }
            };
        }
    }
}
