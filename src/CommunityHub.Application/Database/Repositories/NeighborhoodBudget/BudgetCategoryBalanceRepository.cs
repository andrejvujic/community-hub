using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Database.Mapping.EntityMappers.NeighborhoodBudget;
using CommunityHub.Application.Domain.NeighborhoodBudget;

namespace CommunityHub.Application.Database.Repositories.NeighborhoodBudget
{
    public class BudgetCategoryBalanceRepository
    {
        private readonly BudgetCategoryBalanceMapper _budgetCategoryBalanceMapper = new BudgetCategoryBalanceMapper();

        private const string SelectColumns = @"
            budget_category_balance.id AS budget_category_balance_id,
            budget_category_balance.category AS budget_category,
            budget_category_balance.total_amount,

            neighborhood.id AS neighborhood_id,
            neighborhood.name AS neighborhood_name,
            neighborhood.description,
            neighborhood.budget,

            city.id AS city_id,
            city.name AS city_name,

            country.id AS country_id,
            country.name AS country_name,

            coordinator.id AS coordinator_user_id,
            coordinator.username AS coordinator_username,
            coordinator.password AS coordinator_password,
            coordinator.name AS coordinator_name,
            coordinator.surname AS coordinator_surname,
            coordinator.user_role AS coordinator_user_role";

        private const string FromJoins = @"
            FROM budget_category_balances budget_category_balance
            INNER JOIN neighborhoods neighborhood ON neighborhood.id = budget_category_balance.neighborhood_id
            INNER JOIN cities city ON city.id = neighborhood.city_id
            INNER JOIN countries country ON country.id = city.country_id
            INNER JOIN users coordinator ON coordinator.id = neighborhood.coordinator_id";

        public List<BudgetCategoryBalance> GetByNeighborhoodId(long neighborhoodId)
        {
            var query = PostgresQuery.Create(
                $@"SELECT {SelectColumns} {FromJoins} WHERE budget_category_balance.neighborhood_id = @neighborhood_id ORDER BY budget_category_balance.category"
            ).AddParameter("neighborhood_id", neighborhoodId);

            List<BudgetCategoryBalance> budgetCategoryBalances = new List<BudgetCategoryBalance>();
            query.ExecuteReader(reader =>
            {
                var budgetCategoryBalance = _budgetCategoryBalanceMapper.MapToEntity(reader);
                if (budgetCategoryBalance is not null) budgetCategoryBalances.Add(budgetCategoryBalance);
            });
            return budgetCategoryBalances;
        }
    }
}
