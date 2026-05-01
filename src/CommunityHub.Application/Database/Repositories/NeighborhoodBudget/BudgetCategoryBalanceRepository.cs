using CommunityHub.Application.Database.Mapping.EntityMappers.NeighborhoodBudget;
using CommunityHub.Application.Domain.NeighborhoodBudget;

namespace CommunityHub.Application.Database.Repositories.NeighborhoodBudget
{
    public class BudgetCategoryBalanceRepository
    {
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
    }
}
