using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Helpers.Sorting;

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

        private static string GetOrderByClause(RequestOrder order, bool filterByStatus)
        {
            if (filterByStatus)
            {
                return order switch
                {
                    RequestOrder.CreationDateAscending => "ORDER BY neighborhood_access_requests.status ASC, neighborhood_access_requests.creation_date ASC",
                    RequestOrder.CreationDateDescending => "ORDER BY neighborhood_access_requests.status ASC, neighborhood_access_requests.creation_date DESC",
                    _ => "ORDER BY neighborhood_access_requests.status ASC"
                };
            }

            return order switch
            {
                RequestOrder.CreationDateAscending => "ORDER BY neighborhood_access_requests.creation_date ASC",
                RequestOrder.CreationDateDescending => "ORDER BY neighborhood_access_requests.creation_date DESC",
                _ => string.Empty
            };
        }

        public List<NeighborhoodAccessRequest> GetByCitizenId(long citizenId, RequestOrder order = RequestOrder.Unspecified, bool filterByStatus = false)
        {
            var query = PostgresQuery.Create(
                $@"SELECT 
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

                WHERE
                    neighborhood_access_requests.citizen_id = @citizenId
                {GetOrderByClause(order, filterByStatus)}"
            )
            .AddParameter("citizenId", citizenId)
            ;

            List<NeighborhoodAccessRequest> requests = new List<NeighborhoodAccessRequest>();
            query.ExecuteReader(reader => requests.Add(_neighborhoodAccessRequestMapper.MapToEntity(reader)));

            return requests;
        }

        public List<NeighborhoodAccessRequest> GetWaitingByCitizenId(long citizenId)
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

                WHERE neighborhood_access_requests.citizen_id = @citizenId AND neighborhood_access_requests.status = 'waiting'"
            ).AddParameter("citizenId", citizenId);

            List<NeighborhoodAccessRequest> requests = new List<NeighborhoodAccessRequest>();
            query.ExecuteReader(reader => requests.Add(_neighborhoodAccessRequestMapper.MapToEntity(reader)));

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
