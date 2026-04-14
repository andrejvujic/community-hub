using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.Repositories
{
    public class CitizenRepository
    {
        private readonly CitizenMapper _citizenMapper = new CitizenMapper();

        public List<Citizen> GetByNeighborhoodId(long neighborhoodId)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                citizen.id AS user_id,
                citizen.username,
                citizen.password,
                citizen.name,
                citizen.surname,
                citizen.user_role,

                address.id AS address_id,
                address.street_number,
                address.neighborhood,

                street.id AS street_id,
                street.name AS street_name,

                city.id AS city_id,
                city.name AS city_name,

                country.id AS country_id,
                country.name AS country_name,

                membership.id AS membership_id,
                membership.acceptance_date,

                neighborhood.id AS neighborhood_id,
                neighborhood.name AS neighborhood_name,
                neighborhood.description,
                neighborhood.budget,

                coordinator.id AS coordinator_user_id,
                coordinator.username AS coordinator_username,
                coordinator.password AS coordinator_password,
                coordinator.name AS coordinator_name,
                coordinator.surname AS coordinator_surname,
                coordinator.user_role AS coordinator_user_role

                FROM citizens citizen
                INNER JOIN addresses address ON citizen.address_id = address.id
                INNER JOIN streets street ON address.street_id = street.id
                INNER JOIN cities city ON address.city_id = city.id
                INNER JOIN countries country ON city.country_id = country.id
                INNER JOIN neighborhood_memberships membership ON citizen.membership_id = membership.id
                INNER JOIN neighborhoods neighborhood ON membership.neighborhood_id = neighborhood.id
                INNER JOIN users coordinator ON neighborhood.coordinator_id = coordinator.id
                WHERE membership.neighborhood_id = @neighborhoodId"
            )
            .AddParameter("neighborhoodId", neighborhoodId);

            List<Citizen> citizens = new List<Citizen>();
            query.ExecuteReader(reader => citizens.Add(_citizenMapper.MapToEntity(reader)));

            return citizens;
        }

        public Citizen? GetById(long id)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                citizen.id AS user_id,
                citizen.username,
                citizen.password,
                citizen.name,
                citizen.surname,
                citizen.user_role,

                address.id AS address_id,
                address.street_number,
                address.neighborhood,

                street.id AS street_id,
                street.name AS street_name,

                city.id AS city_id,
                city.name AS city_name,

                country.id AS country_id,
                country.name AS country_name,

                membership.id AS membership_id,
                membership.acceptance_date,

                neighborhood.id AS neighborhood_id,
                neighborhood.name AS neighborhood_name,
                neighborhood.description,
                neighborhood.budget,

                coordinator.id AS coordinator_user_id,
                coordinator.username AS coordinator_username,
                coordinator.password AS coordinator_password,
                coordinator.name AS coordinator_name,
                coordinator.surname AS coordinator_surname,
                coordinator.user_role AS coordinator_user_role

                FROM citizens citizen
                INNER JOIN addresses address ON citizen.address_id = address.id
                INNER JOIN streets street ON address.street_id = street.id
                INNER JOIN cities city ON address.city_id = city.id
                INNER JOIN countries country ON city.country_id = country.id
                LEFT JOIN neighborhood_memberships membership ON citizen.membership_id = membership.id
                LEFT JOIN neighborhoods neighborhood ON membership.neighborhood_id = neighborhood.id
                LEFT JOIN users coordinator ON neighborhood.coordinator_id = coordinator.id
                WHERE citizen.id = @id"
            )
            .AddParameter("id", id);

            Citizen? citizen = null;
            query.ExecuteReader(reader => citizen = _citizenMapper.MapToEntity(reader));

            return citizen;
        }

        public void UpdateMembership(long citizenId, long? membershipId)
        {
            var query = PostgresQuery.Create(
                "UPDATE citizens SET membership_id = @membershipId WHERE id = @citizenId"
            )
            .AddParameter("membershipId", membershipId)
            .AddParameter("citizenId", citizenId);

            query.GetRowsAffectedCount();
        }
    }
}
