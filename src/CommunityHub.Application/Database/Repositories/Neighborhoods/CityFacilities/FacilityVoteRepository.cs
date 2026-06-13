using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.Neighborhoods.CityFacilities;
using CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.CityFacilities;

namespace CommunityHub.Application.Database.Repositories.Neighborhoods.CityFacilities
{
    public class FacilityVoteRepository : IFacilityVoteRepository
    {
        private readonly FacilityVoteMapper _facilityVoteMapper = new FacilityVoteMapper();

        private const string SelectColumns = @"
            facility_vote.id AS facility_vote_id,

            city_facility.id AS city_facility_id,
            city_facility.name AS city_facility_name,

            facility_city.id AS facility_city_id,
            facility_city.name AS facility_city_name,

            facility_country.id AS facility_country_id,
            facility_country.name AS facility_country_name,

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
            coordinator.user_role AS coordinator_user_role";

        private const string FromJoins = @"
            FROM facility_votes facility_vote
            INNER JOIN city_facilities city_facility ON city_facility.id = facility_vote.facility_id
            INNER JOIN cities facility_city ON facility_city.id = city_facility.city_id
            INNER JOIN countries facility_country ON facility_country.id = facility_city.country_id
            INNER JOIN citizens citizen ON citizen.id = facility_vote.citizen_id
            INNER JOIN addresses address ON citizen.address_id = address.id
            INNER JOIN streets street ON address.street_id = street.id
            INNER JOIN cities city ON address.city_id = city.id
            INNER JOIN countries country ON city.country_id = country.id
            LEFT JOIN neighborhood_memberships membership ON citizen.membership_id = membership.id
            LEFT JOIN neighborhoods neighborhood ON membership.neighborhood_id = neighborhood.id
            LEFT JOIN users coordinator ON neighborhood.coordinator_id = coordinator.id";

        public FacilityVote? GetById(long id)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                $"SELECT {SelectColumns} {FromJoins} WHERE facility_vote.id = @id"
            ).AddParameter("id", id);

            FacilityVote? vote = null;
            connection.ExecuteReader(query, reader =>
            {
                vote = _facilityVoteMapper.MapToEntity(reader);
            });
            return vote;
        }

        public List<FacilityVote> GetByNeighborhoodId(long neighborhoodId)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                $"SELECT {SelectColumns} {FromJoins} WHERE neighborhood.id = @neighborhood_id ORDER BY facility_vote.id"
            ).AddParameter("neighborhood_id", neighborhoodId);

            List<FacilityVote> votes = new List<FacilityVote>();
            connection.ExecuteReader(query, reader =>
            {
                var vote = _facilityVoteMapper.MapToEntity(reader);
                if (vote is not null) votes.Add(vote);
            });
            return votes;
        }

        public FacilityVote? GetByFacilityIdAndCitizenId(long facilityId, long citizenId)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                $"SELECT {SelectColumns} {FromJoins} WHERE city_facility.id = @facility_id AND citizen.id = @citizen_id"
            ).AddParameter("facility_id", facilityId)
             .AddParameter("citizen_id", citizenId);

            FacilityVote? vote = null;
            connection.ExecuteReader(query, reader =>
            {
                vote = _facilityVoteMapper.MapToEntity(reader);
            });
            return vote;
        }

        public int GetVoteCountByFacilityAndNeighborhoodId(long facilityId, long neighborhoodId)
        {
            using var connection = DatabaseConnection.Create();
            var query = DatabaseQuery.Create(
                @"SELECT COUNT(*) 
                FROM facility_votes fv
                INNER JOIN citizens c ON c.id = fv.citizen_id
                INNER JOIN neighborhood_memberships nm ON nm.id = c.membership_id
                WHERE fv.facility_id = @facility_id
                AND nm.neighborhood_id = @neighborhood_id"
            )
            .AddParameter("facility_id", facilityId)
            .AddParameter("neighborhood_id", neighborhoodId);

            return connection.ExecuteScalar(query, value => value != null ? Convert.ToInt32(value) : 0);
        }

        public FacilityVote Create(FacilityVote vote)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                @"INSERT INTO facility_votes (facility_id, citizen_id)
                VALUES (@facility_id, @citizen_id)
                RETURNING id"
            )
            .ExtendParameters(_facilityVoteMapper.MapToQueryParameters(vote));

            long id = connection.ExecuteScalar(query, Convert.ToInt64);

            return vote.CopyWith(id: id);
        }

        public void DeleteByFacilityIdAndCitizenId(long facilityId, long citizenId)
        {
            using var connection = DatabaseConnection.Create();
            var query = DatabaseQuery.Create(
                "DELETE FROM facility_votes WHERE facility_id = @facility_id AND citizen_id = @citizen_id"
            )
            .AddParameter("facility_id", facilityId)
            .AddParameter("citizen_id", citizenId);

            connection.GetRowsAffectedCount(query);
        }

        public void DeleteAllByFacilityId(long facilityId)
        {
            using var connection = DatabaseConnection.Create();
            var query = DatabaseQuery.Create(
                "DELETE FROM facility_votes WHERE facility_id = @facility_id"
            )
            .AddParameter("facility_id", facilityId);

            connection.GetRowsAffectedCount(query);
        }
    }
}
