using CommunityHub.Application.Database.Mapping.EntityMappers.NeighborhoodBudget;
using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.NeighborhoodBudgets;

namespace CommunityHub.Application.Database.Repositories.Neighborhoods.NeighborhoodBudgets
{
    public class BudgetDonationRepository : IBudgetDonationRepository
    {
        private readonly BudgetDonationMapper _budgetDonationMapper = new BudgetDonationMapper();

        private const string SelectColumns = @"
            budget_donation.id AS budget_donation_id,
            budget_donation.amount AS donation_amount,
            budget_donation.donation_date,

            neighborhood.id AS neighborhood_id,
            neighborhood.name AS neighborhood_name,
            neighborhood.description,
            neighborhood.budget,

            budget_category.id AS budget_category_id,
            budget_category.name AS budget_category_name,

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

            citizen.id AS citizen_id,
            citizen.username AS citizen_username,
            citizen.password AS citizen_password,
            citizen.name AS citizen_name,
            citizen.surname AS citizen_surname,
            citizen.user_role AS citizen_user_role,
            citizen.address_id,
            citizen.membership_id";

        private const string FromJoins = @"
            FROM budget_donations budget_donation
            INNER JOIN neighborhoods neighborhood ON neighborhood.id = budget_donation.neighborhood_id
            INNER JOIN cities city ON city.id = neighborhood.city_id
            INNER JOIN countries country ON country.id = city.country_id
            INNER JOIN users coordinator ON coordinator.id = neighborhood.coordinator_id
            INNER JOIN budget_categories budget_category ON budget_category.id = budget_donation.budget_category_id
            INNER JOIN citizens citizen ON citizen.id = budget_donation.citizen_id";

        public List<BudgetDonation> GetByNeighborhoodId(long neighborhoodId)
        {
            var query = PostgresQuery.Create(
                $@"SELECT {SelectColumns} 
                {FromJoins} 
                WHERE budget_donation.neighborhood_id = @neighborhood_id 
                ORDER BY budget_donation.donation_date DESC"
            ).AddParameter("neighborhood_id", neighborhoodId);

            List<BudgetDonation> budgetDonations = new List<BudgetDonation>();
            query.ExecuteReader(reader =>
            {
                var budgetDonation = _budgetDonationMapper.MapToEntity(reader);
                if (budgetDonation is not null) budgetDonations.Add(budgetDonation);
            });
            return budgetDonations;
        }

        public List<BudgetDonation> GetByNeighborhoodIdAndCategoryId(long neighborhoodId, long categoryId)
        {
            var query = PostgresQuery.Create(
                $@"SELECT {SelectColumns} 
                {FromJoins} 
                WHERE budget_donation.neighborhood_id = @neighborhood_id 
                AND budget_donation.budget_category_id = @category_id 
                ORDER BY budget_donation.donation_date DESC"
            )
            .AddParameter("neighborhood_id", neighborhoodId)
            .AddParameter("category_id", categoryId);

            List<BudgetDonation> budgetDonations = new List<BudgetDonation>();
            query.ExecuteReader(reader =>
            {
                var budgetDonation = _budgetDonationMapper.MapToEntity(reader);
                if (budgetDonation is not null) budgetDonations.Add(budgetDonation);
            });
            return budgetDonations;
        }

        public BudgetDonation Create(BudgetDonation budgetDonation)
        {
            var query = PostgresQuery.Create(
                @"INSERT INTO budget_donations 
                (amount, donation_date, neighborhood_id, budget_category_id, citizen_id)
                VALUES 
                (@amount, @donation_date, @neighborhood_id, @budget_category_id, @citizen_id)
                RETURNING id"
            )
            .ExtendParameters(_budgetDonationMapper.MapToQueryParameters(budgetDonation));

            long budgetDonationId = query.ExecuteScalar<long>(value => Convert.ToInt64(value));

            return budgetDonation.CopyWith(id: budgetDonationId);
        }
    }
}
