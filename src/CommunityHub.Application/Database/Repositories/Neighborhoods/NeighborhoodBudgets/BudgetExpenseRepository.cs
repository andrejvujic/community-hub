using CommunityHub.Application.Database.Mapping.EntityMappers.NeighborhoodBudget;
using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.NeighborhoodBudgets;

namespace CommunityHub.Application.Database.Repositories.Neighborhoods.NeighborhoodBudgets
{
    public class BudgetExpenseRepository : IBudgetExpenseRepository
    {

        private readonly BudgetExpenseMapper _budgetExpenseMapper = new BudgetExpenseMapper();

        private const string SelectColumns = @"
            budget_expense.id AS budget_expense_id,
            budget_expense.amount AS expense_amount,
            budget_expense.description AS expense_description,
            budget_expense.expense_date,

            budget_category.id AS budget_category_id,
            budget_category.name AS budget_category_name,

            budget_category_balance.id AS budget_category_balance_id,
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

        private const string FromJoins = @"
            FROM budget_expenses budget_expense
            INNER JOIN budget_category_balances budget_category_balance ON budget_category_balance.id = budget_expense.budget_category_balance_id
            INNER JOIN budget_categories budget_category ON budget_category.id = budget_category_balance.category_id
            INNER JOIN neighborhoods neighborhood ON neighborhood.id = budget_category_balance.neighborhood_id
            INNER JOIN cities city ON city.id = neighborhood.city_id
            INNER JOIN countries country ON country.id = city.country_id
            INNER JOIN users coordinator ON coordinator.id = neighborhood.coordinator_id
            INNER JOIN users expense_coordinator ON expense_coordinator.id = budget_expense.coordinator_id";

        public List<BudgetExpense> GetByNeighborhoodId(long neighborhoodId)
        {
            var query = PostgresQuery.Create(
                $@"SELECT {SelectColumns} 
                {FromJoins} 
                WHERE budget_category_balance.neighborhood_id = @neighborhood_id 
                ORDER BY budget_expense.expense_date DESC"
            ).AddParameter("neighborhood_id", neighborhoodId);

            List<BudgetExpense> budgetExpenses = new List<BudgetExpense>();
            query.ExecuteReader(reader =>
            {
                var budgetExpense = _budgetExpenseMapper.MapToEntity(reader);
                if (budgetExpense is not null) budgetExpenses.Add(budgetExpense);
            });
            return budgetExpenses;
        }

        public List<BudgetExpense> GetByCategoryId(long categoryId)
        {
            var query = PostgresQuery.Create(
                $@"SELECT {SelectColumns} 
                {FromJoins} 
                WHERE budget_category.id = @category_id 
                ORDER BY budget_expense.expense_date DESC"
            ).AddParameter("category_id", categoryId);

            List<BudgetExpense> budgetExpenses = new List<BudgetExpense>();
            query.ExecuteReader(reader =>
            {
                var budgetExpense = _budgetExpenseMapper.MapToEntity(reader);
                if (budgetExpense is not null) budgetExpenses.Add(budgetExpense);
            });
            return budgetExpenses;
        }

        public BudgetExpense Create(BudgetExpense budgetExpense)
        {
            var query = PostgresQuery.Create(
                @"INSERT INTO budget_expenses 
                (amount, description, expense_date, budget_category_balance_id, coordinator_id)
                VALUES 
                (@amount, @description, @expense_date, @budget_category_balance_id, @coordinator_id)
                RETURNING id"
            )
            .ExtendParameters(_budgetExpenseMapper.MapToQueryParameters(budgetExpense));
            
            long budgetExpenseId = query.ExecuteScalar<long>(value => Convert.ToInt64(value));

            return budgetExpense.CopyWith(id: budgetExpenseId);
        }
    }
}
