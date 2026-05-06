using System.Data;
using CommunityHub.Application.Domain.NeighborhoodBudget;
using CommunityHub.Application.Modules.Shared.Core.Domain.Users;

namespace CommunityHub.Application.Database.Mapping.EntityMappers.NeighborhoodBudget
{
    public class BudgetExpenseMapper : EntityMapper<BudgetExpense>
    {
        private readonly BudgetCategoryBalanceMapper _balanceMapper = new BudgetCategoryBalanceMapper();
        private readonly UserMapper _userMapper = new UserMapper();

        public override BudgetExpense? MapToEntity(IDataReader reader)
        {
            int idOrdinal = reader.GetOrdinal("budget_expense_id");
            if (reader.IsDBNull(idOrdinal)) return null;
            long id = reader.GetInt64(idOrdinal);

            decimal amount = reader.GetDecimal(reader.GetOrdinal("expense_amount"));
            string description = reader.GetString(reader.GetOrdinal("expense_description"));
            DateOnly expenseDate = DateOnly.FromDateTime(
                reader.GetDateTime(reader.GetOrdinal("expense_date")));

            BudgetCategoryBalance? balance = _balanceMapper.MapToEntity(reader);
            if (balance is null) return null;

            User? coordinator = _userMapper.MapToEntity(reader.WithPrefix("expense_coordinator"));
            if (coordinator is null) return null;

            return new BudgetExpense(balance, coordinator, amount, description, expenseDate, id);
        }

        public override Dictionary<string, object?> MapToQueryParameters(BudgetExpense entity)
        {
            return new Dictionary<string, object?>
            {
                { "amount", entity.Amount },
                { "description", entity.Description },
                { "expense_date", entity.ExpenseDate.ToDateTime(TimeOnly.MinValue) },
                { "budget_category_balance_id", entity.BudgetCategoryBalance.Id },
                { "coordinator_id", entity.Coordinator.Id }
            };
        }
    }
}
