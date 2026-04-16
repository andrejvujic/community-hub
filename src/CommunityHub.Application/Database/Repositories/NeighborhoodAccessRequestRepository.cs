using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.Repositories
{
    public class NeighborhoodAccessRequestRepository
    {
        private readonly NeighborhoodAccessRequestMapper _neighborhoodAccessRequestMapper = new NeighborhoodAccessRequestMapper();

        public List<NeighborhoodAccessRequest> GetWaitingByNeighborhoodId(long neighborhoodId)
        {
            var query = PostgresQuery.Create(
                @"SELECT 
                neighborhood_access_requests.status, 
                neighborhood_access_requests.creation_date, 
                neighborhood_access_requests.comment,

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

                address_city.id AS city_id, 
                address_city.name AS city_name,

                neighborhood_memberships.id AS membership_id, 
                neighborhood_memberships.acceptance_date,

                neighborhoods.id AS neighborhood_id, 
                neighborhoods.name AS neighborhood_name, 
                neighborhoods.description, 
                neighborhoods.budget,

                neighborhood_city.id AS city_id, 
                neighborhood_city.name AS city_name

                FROM neighborhood_access_requests
                INNER JOIN citizens ON neighborhood_access_requests.citizen_id = citizens.id
                INNER JOIN neighborhoods ON neighborhood_access_requests.neighborhood_id = neighborhoods.id
        
                INNER JOIN cities AS neighborhood_city ON neighborhoods.city_id = neighborhood_city.id
        
                LEFT JOIN addresses ON citizens.address_id = addresses.id
                LEFT JOIN streets ON addresses.street_id = streets.id
        
                LEFT JOIN cities AS address_city ON addresses.city_id = address_city.id
        
                LEFT JOIN neighborhood_memberships ON citizens.membership_id = neighborhood_memberships.id

                WHERE neighborhood_access_requests.neighborhood_id = @neighborhoodId 
                AND neighborhood_access_requests.status = 'waiting'"
            ).AddParameter("neighborhoodId", neighborhoodId);

            List<NeighborhoodAccessRequest> requests = new List<NeighborhoodAccessRequest>();
            query.ExecuteReader(reader => requests.Add(_neighborhoodAccessRequestMapper.MapToEntity(reader)));

            return requests;
        }

        public List<NeighborhoodAccessRequest> GetByCitizenId(long citizenId)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                    citizen_id,
                    neighborhood_id,
                    status,
                    creation_date,
                    comment
                FROM neighborhood_access_requests
                WHERE citizen_id = @citizenId"
            ).AddParameter("citizenId", citizenId);

            var requests = new List<NeighborhoodAccessRequest>();

            var citizenRepo = new CitizenRepository();
            var neighborhoodMapper = new CommunityHub.Application.Database.Mapping.EntityMappers.NeighborhoodMapper();
            var neighborhoodCache = new Dictionary<long, Neighborhood>();

            query.ExecuteReader(reader => {
                long cId = reader.GetInt64(reader.GetOrdinal("citizen_id"));
                long nId = reader.GetInt64(reader.GetOrdinal("neighborhood_id"));

                Status status = (Status)reader.GetValue(reader.GetOrdinal("status"));
                DateTime creationDate = reader.GetDateTime(reader.GetOrdinal("creation_date"));

                int commentIdx = reader.GetOrdinal("comment");
                string? comment = reader.IsDBNull(commentIdx) ? null : reader.GetString(commentIdx);

                Citizen? citizen = citizenRepo.GetById(cId);
                if (citizen == null) return;

                if (!neighborhoodCache.TryGetValue(nId, out Neighborhood? neighborhood))
                {
                    var nQuery = PostgresQuery.Create(
                        @"SELECT
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
                            coordinator.user_role AS coordinator_user_role
                        FROM neighborhoods neighborhood
                        INNER JOIN cities city ON neighborhood.city_id = city.id
                        INNER JOIN countries country ON city.country_id = country.id
                        INNER JOIN users coordinator ON neighborhood.coordinator_id = coordinator.id
                        WHERE neighborhood.id = @id"
                    ).AddParameter("id", nId);

                    Neighborhood? fetched = null;
                    nQuery.ExecuteReader(r => fetched = neighborhoodMapper.MapToEntity(r));
                    if (fetched == null) return;

                    neighborhoodCache[nId] = fetched;
                    neighborhood = fetched;
                }

                requests.Add(new NeighborhoodAccessRequest(citizen, neighborhood, status, creationDate, comment));
            });

            return requests;
        }

        public List<NeighborhoodAccessRequest> GetWaitingByCitizenId(long citizenId)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                    citizen_id,
                    neighborhood_id,
                    status,
                    creation_date,
                    comment
                FROM neighborhood_access_requests
                WHERE citizen_id = @citizenId AND status = 'waiting'"
            ).AddParameter("citizenId", citizenId);

            var requests = new List<NeighborhoodAccessRequest>();

            var citizenRepo = new CitizenRepository();
            var neighborhoodMapper = new CommunityHub.Application.Database.Mapping.EntityMappers.NeighborhoodMapper();
            var neighborhoodCache = new Dictionary<long, Neighborhood>();

            query.ExecuteReader(reader => {
                long cId = reader.GetInt64(reader.GetOrdinal("citizen_id"));
                long nId = reader.GetInt64(reader.GetOrdinal("neighborhood_id"));

                Status status = (Status)reader.GetValue(reader.GetOrdinal("status"));
                DateTime creationDate = reader.GetDateTime(reader.GetOrdinal("creation_date"));

                int commentIdx = reader.GetOrdinal("comment");
                string? comment = reader.IsDBNull(commentIdx) ? null : reader.GetString(commentIdx);

                Citizen? citizen = citizenRepo.GetById(cId);
                if (citizen == null) return;

                if (!neighborhoodCache.TryGetValue(nId, out Neighborhood? neighborhood))
                {
                    var nQuery = PostgresQuery.Create(
                        @"SELECT
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
                            coordinator.user_role AS coordinator_user_role
                        FROM neighborhoods neighborhood
                        INNER JOIN cities city ON neighborhood.city_id = city.id
                        INNER JOIN countries country ON city.country_id = country.id
                        INNER JOIN users coordinator ON neighborhood.coordinator_id = coordinator.id
                        WHERE neighborhood.id = @id"
                    ).AddParameter("id", nId);

                    Neighborhood? fetched = null;
                    nQuery.ExecuteReader(r => fetched = neighborhoodMapper.MapToEntity(r));
                    if (fetched == null) return;

                    neighborhoodCache[nId] = fetched;
                    neighborhood = fetched;
                }

                requests.Add(new NeighborhoodAccessRequest(citizen, neighborhood, status, creationDate, comment));
            });

            return requests;
        }

        public List<NeighborhoodAccessRequest> GetByCoordinatorId(long coordinatorId)
        {
            var query = PostgresQuery.Create(
                @"SELECT 
                neighborhood_access_requests.status, 
                neighborhood_access_requests.creation_date, 
                neighborhood_access_requests.comment,
        
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
        
                coordinator_users.id AS coordinator_user_id,
                coordinator_users.username AS coordinator_username,
                coordinator_users.password AS coordinator_password,
                coordinator_users.name AS coordinator_name,
                coordinator_users.surname AS coordinator_surname,
                coordinator_users.user_role AS coordinator_user_role

                FROM neighborhood_access_requests
                INNER JOIN citizens ON neighborhood_access_requests.citizen_id = citizens.id
                INNER JOIN neighborhoods ON neighborhood_access_requests.neighborhood_id = neighborhoods.id
                INNER JOIN users AS coordinator_users ON neighborhoods.coordinator_id = coordinator_users.id
                INNER JOIN cities ON neighborhoods.city_id = cities.id
                INNER JOIN countries ON cities.country_id = countries.id
    
                LEFT JOIN addresses ON citizens.address_id = addresses.id
                LEFT JOIN streets ON addresses.street_id = streets.id
                LEFT JOIN neighborhood_memberships ON citizens.membership_id = neighborhood_memberships.id
    
                WHERE neighborhoods.coordinator_id = @coordinatorId"
            ).AddParameter("coordinatorId", coordinatorId);

            List<NeighborhoodAccessRequest> requests = new List<NeighborhoodAccessRequest>();
            query.ExecuteReader(reader => requests.Add(_neighborhoodAccessRequestMapper.MapToEntity(reader)));

            return requests;
        }

        public List<NeighborhoodAccessRequest> GetByCoordinatorIdAndStatus(long coordinatorId, Status? status)
        {
            string queryText = @"
                SELECT 
                neighborhood_access_requests.status, 
                neighborhood_access_requests.creation_date, 
                neighborhood_access_requests.comment,
        
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
        
                coordinator_users.id AS coordinator_user_id,
                coordinator_users.username AS coordinator_username,
                coordinator_users.password AS coordinator_password,
                coordinator_users.name AS coordinator_name,
                coordinator_users.surname AS coordinator_surname,
                coordinator_users.user_role AS coordinator_user_role

                FROM neighborhood_access_requests
                INNER JOIN citizens ON neighborhood_access_requests.citizen_id = citizens.id
                INNER JOIN neighborhoods ON neighborhood_access_requests.neighborhood_id = neighborhoods.id
                INNER JOIN users AS coordinator_users ON neighborhoods.coordinator_id = coordinator_users.id
                INNER JOIN cities ON neighborhoods.city_id = cities.id
                INNER JOIN countries ON cities.country_id = countries.id
    
                LEFT JOIN addresses ON citizens.address_id = addresses.id
                LEFT JOIN streets ON addresses.street_id = streets.id
                LEFT JOIN neighborhood_memberships ON citizens.membership_id = neighborhood_memberships.id
    
                WHERE neighborhoods.coordinator_id = @coordinatorId";

            if (status.HasValue)
            {
                queryText += " AND neighborhood_access_requests.status = @status";
            }

            var query = PostgresQuery.Create(queryText)
                .AddParameter("coordinatorId", coordinatorId);

            if (status.HasValue)
            {
                query.AddParameter("status", status.Value);
            }

            List<NeighborhoodAccessRequest> requests = new List<NeighborhoodAccessRequest>();
            query.ExecuteReader(reader => requests.Add(_neighborhoodAccessRequestMapper.MapToEntity(reader)));

            return requests;
        }

        public void Update(NeighborhoodAccessRequest request)
        {
            var query = PostgresQuery.Create(
                @"UPDATE neighborhood_access_requests 
                    SET status = @status, 
                    comment = @comment 
                    WHERE citizen_id = @citizen_id AND neighborhood_id = @neighborhood_id"
            )
            .ExtendParameters(_neighborhoodAccessRequestMapper.MapToQueryParameters(request));

            query.GetRowsAffectedCount();
        }

        public bool Create(NeighborhoodAccessRequest request)
        {
            var query = PostgresQuery.Create(
                @"INSERT INTO neighborhood_access_requests
                (citizen_id, neighborhood_id, comment, status, creation_date)
                VALUES
                (@citizen_id, @neighborhood_id, @comment, @status, @creation_date)"
            )
            .ExtendParameters(_neighborhoodAccessRequestMapper.MapToQueryParameters(request));

            int affected = query.GetRowsAffectedCount();
            return affected > 0;
        }

        public void Delete(long citizenId, long neighborhoodId)
        {
            PostgresQuery.Create(
                "DELETE FROM neighborhood_access_requests WHERE citizen_id = @citizen_id AND neighborhood_id = @neighborhood_id"
            )
            .AddParameter("citizen_id", citizenId)
            .AddParameter("neighborhood_id", neighborhoodId)
            .GetRowsAffectedCount();
        }

        public bool HasPendingRequest(long userId, long neighborhoodId)
        {
            var query = PostgresQuery.Create(
                @"SELECT request.citizen_id
                FROM neighborhood_access_requests request
                WHERE
                    request.citizen_id = @user_id AND
                    request.neighborhood_id = @neighborhood_id AND
                    request.status = 'waiting'"
            )
            .AddParameter("user_id", userId)
            .AddParameter("neighborhood_id", neighborhoodId);

            return query.ExecuteScalar(val => val != null);
        }
    }
}
