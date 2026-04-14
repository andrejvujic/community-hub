using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.Repositories
{
    public class NeighborhoodMembershipRepository
    {
        private readonly NeighborhoodMembershipMapper _neighborhoodMembershipMapper = new NeighborhoodMembershipMapper();
        private readonly CitizenMapper _citizenMapper = new CitizenMapper();

        public NeighborhoodMembership Create(NeighborhoodMembership membership)
        {
            var query = PostgresQuery.Create(
                @"INSERT INTO neighborhood_memberships (acceptance_date, neighborhood_id) 
                VALUES (@acceptance_date, @neighborhood_id) 
                RETURNING id"
            ).ExtendParameters(_neighborhoodMembershipMapper.MapToQueryParameters(membership));

            long generatedId = query.ExecuteScalar<long>(value => Convert.ToInt64(value));
            
            return membership.CopyWith(id: generatedId);
        }

        public List<Citizen> GetCitizensByNeighborhoodId(long neighborhoodId)
        {
            var query = PostgresQuery.Create(
                @"SELECT 
                citizens.id AS user_id, 
                citizens.username, 
                citizens.password, 
                citizens.name, 
                citizens.surname, 
                citizens.user_role,

                addresses.id AS address_id, 
                addresses.street_number, 
                addresses.neighborhood,
                streets.id AS street_id, 
                streets.name AS street_name,
                cities.id AS city_id, 
                cities.name AS city_name,
                countries.id AS country_id, 
                countries.name AS country_name,

                neighborhood_memberships.id AS membership_id, 
                neighborhood_memberships.acceptance_date,

                neighborhoods.id AS neighborhood_id, 
                neighborhoods.name AS neighborhood_name, 
                neighborhoods.description, 
                neighborhoods.budget,

                coordinators.id AS coordinator_user_id,
                coordinators.username AS coordinator_username,
                coordinators.password AS coordinator_password,
                coordinators.name AS coordinator_name,
                coordinators.surname AS coordinator_surname,
                coordinators.user_role AS coordinator_user_role

                FROM citizens
                INNER JOIN addresses ON citizens.address_id = addresses.id
                INNER JOIN streets ON addresses.street_id = streets.id
                INNER JOIN cities ON addresses.city_id = cities.id
                INNER JOIN countries ON cities.country_id = countries.id
                INNER JOIN neighborhood_memberships ON citizens.membership_id = neighborhood_memberships.id
                INNER JOIN neighborhoods ON neighborhood_memberships.neighborhood_id = neighborhoods.id
                INNER JOIN users AS coordinators ON neighborhoods.coordinator_id = coordinators.id
                WHERE neighborhood_memberships.neighborhood_id = @id"
            ).AddParameter("id", neighborhoodId);

            List<Citizen> members = new List<Citizen>();
            query.ExecuteReader(reader => {
                members.Add(_citizenMapper.MapToEntity(reader));
            });
            return members;
        }
    }
}
