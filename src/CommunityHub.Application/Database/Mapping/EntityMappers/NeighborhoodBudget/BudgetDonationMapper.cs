using System.Data;
using CommunityHub.Application.Domain.Neighborhoods;
using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.Database.Mapping.EntityMappers.NeighborhoodBudget
{
    public class BudgetDonationMapper : EntityMapper<BudgetDonation>
    {
        private readonly NeighborhoodMapper _neighborhoodMapper = new NeighborhoodMapper();
        private readonly BudgetCategoryMapper _budgetCategoryMapper = new BudgetCategoryMapper();
        private readonly CitizenMapper _citizenMapper = new CitizenMapper();

        public override BudgetDonation? MapToEntity(IDataReader reader)
        {
            int idOrdinal = reader.GetOrdinal("budget_donation_id");
            if (reader.IsDBNull(idOrdinal)) return null;
            long id = reader.GetInt64(idOrdinal);

            decimal amount = reader.GetDecimal(reader.GetOrdinal("donation_amount"));
            DateOnly donationDate = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("donation_date")));

            Neighborhood neighborhood = _neighborhoodMapper.MapToEntity(reader);
            BudgetCategory budgetCategory = _budgetCategoryMapper.MapToEntity(reader);
            Citizen? citizen = _citizenMapper.MapToEntity(reader);
            if (citizen is null) return null;

            return new BudgetDonation(neighborhood, budgetCategory, citizen, amount, donationDate, id);
        }

        public override Dictionary<string, object?> MapToQueryParameters(BudgetDonation entity)
        {
            return new Dictionary<string, object?>
            {
                { "amount", entity.Amount },
                { "donation_date", entity.DonationDate.ToDateTime(TimeOnly.MinValue) },
                { "neighborhood_id", entity.Neighborhood.Id },
                { "budget_category_id", entity.BudgetCategory.Id },
                { "citizen_id", entity.Citizen.Id }
            };
        }
    }
}
