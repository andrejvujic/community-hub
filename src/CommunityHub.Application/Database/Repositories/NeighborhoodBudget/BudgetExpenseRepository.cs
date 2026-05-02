namespace CommunityHub.Application.Database.Repositories.NeighborhoodBudget
{
    public class BudgetExpenseRepository
    {
        private const string SelectColumns = @"
            budget_expense.id AS budget_expense_id,
            budget_expense.amount AS expense_amount,
            budget_expense.description AS expense_description,
            budget_expense.expense_date,

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
            coordinator.user_role AS coordinator_user_role,

            expense_coordinator.id AS expense_coordinator_user_id,
            expense_coordinator.username AS expense_coordinator_username,
            expense_coordinator.password AS expense_coordinator_password,
            expense_coordinator.name AS expense_coordinator_name,
            expense_coordinator.surname AS expense_coordinator_surname,
            expense_coordinator.user_role AS expense_coordinator_user_role";
    }
}
