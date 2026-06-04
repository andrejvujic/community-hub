using System.Data;
using CommunityHub.Application.Domain.Neighborhoods;
using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodBudgets;

namespace CommunityHub.Application.Database.Mapping.EntityMappers.NeighborhoodBudget
{
    public class BudgetCategoryBalanceMapper : EntityMapper<BudgetCategoryBalance>
    {
        private readonly NeighborhoodMapper _neighborhoodMapper = new NeighborhoodMapper();
        private readonly BudgetCategoryMapper _budgetCategoryMapper = new BudgetCategoryMapper();

        public override BudgetCategoryBalance? MapToEntity(IDataReader reader)
        {
            int idOrdinal = reader.GetOrdinal("budget_category_balance_id");
            if (reader.IsDBNull(idOrdinal)) return null;
            long id = reader.GetInt64(idOrdinal);
            BudgetCategory budgetCategory = _budgetCategoryMapper.MapToEntity(reader);
            decimal totalAmount = reader.GetDecimal(reader.GetOrdinal("total_amount"));
            Neighborhood neighborhood = _neighborhoodMapper.MapToEntity(reader);

            return new BudgetCategoryBalance(neighborhood, budgetCategory, totalAmount, id);
        }

        public override Dictionary<string, object?> MapToQueryParameters(BudgetCategoryBalance entity)
        {
            return new Dictionary<string, object?>
            {
                { "budget_category_id", entity.Category.Id },
                { "neighborhood_id", entity.Neighborhood.Id },
                { "total_amount", entity.TotalAmount }
            };
        }
    }
}
