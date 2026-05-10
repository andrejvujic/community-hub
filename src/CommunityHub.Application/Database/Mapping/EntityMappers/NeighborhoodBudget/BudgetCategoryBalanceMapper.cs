using System.Data;
using CommunityHub.Application.Domain.NeighborhoodBudget;
using CommunityHub.Application.Domain.Neighborhoods;

namespace CommunityHub.Application.Database.Mapping.EntityMappers.NeighborhoodBudget
{
    public class BudgetCategoryBalanceMapper : EntityMapper<BudgetCategoryBalance>
    {
        private readonly NeighborhoodMapper _neighborhoodMapper = new NeighborhoodMapper();

        public override BudgetCategoryBalance? MapToEntity(IDataReader reader)
        {
            int idOrdinal = reader.GetOrdinal("budget_category_balance_id");
            if (reader.IsDBNull(idOrdinal)) return null;
            long id = reader.GetInt64(idOrdinal);

            BudgetCategory category = (BudgetCategory)reader.GetValue(reader.GetOrdinal("budget_category"));
            decimal totalAmount = reader.GetDecimal(reader.GetOrdinal("total_amount"));
            Neighborhood neighborhood = _neighborhoodMapper.MapToEntity(reader);

            return new BudgetCategoryBalance(neighborhood, category, totalAmount, id);
        }

        public override Dictionary<string, object?> MapToQueryParameters(BudgetCategoryBalance entity)
        {
            return new Dictionary<string, object?>
            {
                { "category", entity.Category },
                { "neighborhood_id", entity.Neighborhood.Id },
                { "total_amount", entity.TotalAmount }
            };
        }
    }
}
