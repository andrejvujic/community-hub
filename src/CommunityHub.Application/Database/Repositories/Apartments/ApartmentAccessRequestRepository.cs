using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.Apartments.AccessRequests;
using CommunityHub.Application.Helpers.Sorting;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.RepositoryInterfaces.Apartments;

namespace CommunityHub.Application.Database.Repositories.Apartments
{
    public class ApartmentAccessRequestRepository : IApartmentAccessRequestRepository
    {
        private readonly ApartmentAccessRequestMapper _apartmentAccessRequestMapper = new();

        public string GetOrderByClause(RequestOrder order)
        {
            return order switch
            {
                RequestOrder.CreationDateAscending => "ORDER BY request.creation_date ASC",
                RequestOrder.CreationDateDescending => "ORDER BY request.creation_date DESC",
                _ => string.Empty
            };
        }

        public ApartmentAccessRequest Create(ApartmentAccessRequest request)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                @"INSERT INTO apartment_access_requests
                    (applicant_id, apartment_id, comment, status, creation_date)
                VALUES
                    (@applicant_id, @apartment_id, @comment, @status, @creation_date)
                RETURNING id"
            ).ExtendParameters(
                _apartmentAccessRequestMapper.MapToQueryParameters(request)
            );

            long id = connection.ExecuteScalar(query, Convert.ToInt64);
            return request.CopyWith(id: id);
        }

        public void Delete(long requestId)
        {
            using var connection = DatabaseConnection.Create();

            connection.Execute(
                DatabaseQuery.Create("DELETE FROM apartment_access_requests WHERE id = @request_id")
                .AddParameter("request_id", requestId)
            );
        }

        public List<ApartmentAccessRequest> GetByApplicant(
            long id,
            Status? status = null,
            RequestOrder order = RequestOrder.Unspecified
        )
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                $@"SELECT
                request.id as request_id,
                request.status,
                request.comment,
                request.creation_date,

                applicant.id as applicant_user_id,
                applicant.username as applicant_username,
                applicant.password as applicant_password,
                applicant.name as applicant_name,
                applicant.surname as applicant_surname,
                applicant.user_role as applicant_user_role,

                apartment.id as apartment_id,
                apartment.apartment_number,
                apartment.floor_number,
                
                building.id as building_id,
                building.floor_count,
                building.apartment_count,

                building_manager.id as building_manager_user_id,
                building_manager.username as building_manager_username,
                building_manager.password as building_manager_password,
                building_manager.name as building_manager_name,
                building_manager.surname as building_manager_surname,
                building_manager.user_role as building_manager_user_role,
                
                address.id as address_id,
                address.street_number,
                address.neighborhood,
                
                street.id as street_id,
                street.name as street_name,

                city.id as city_id,
                city.name as city_name,

                country.id as country_id,
                country.name as country_name

                FROM apartment_access_requests request
                INNER JOIN users applicant ON request.applicant_id = applicant.id
                INNER JOIN apartments apartment ON request.apartment_id = apartment.id
                INNER JOIN buildings building ON apartment.building_id = building.id
                INNER JOIN users building_manager ON building.building_manager_id = building_manager.id
                INNER JOIN addresses address ON building.address_id = address.id
                INNER JOIN streets street ON address.street_id = street.id
                INNER JOIN cities city ON address.city_id = city.id
                INNER JOIN countries country ON city.country_id = country.id
                WHERE
                    request.applicant_id = @id AND
                    (request.status = @status OR @status IS NULL)
                {GetOrderByClause(order)}"
            )
            .AddParameter("id", id)
            .AddParameter("status", status);

            var requests = new List<ApartmentAccessRequest>();

            connection.ExecuteReader(
                query,
                reader => requests.Add(
                    _apartmentAccessRequestMapper.MapToEntity(reader)
                )
            );

            return requests;
        }

        public List<ApartmentAccessRequest> GetByApplicantAndApartment(
            long applicantId,
            long apartmentId,
            Status? status = null
        )
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                @"SELECT
                request.id as request_id,
                request.status,
                request.comment,
                request.creation_date,

                applicant.id as applicant_user_id,
                applicant.username as applicant_username,
                applicant.password as applicant_password,
                applicant.name as applicant_name,
                applicant.surname as applicant_surname,
                applicant.user_role as applicant_user_role,

                apartment.id as apartment_id,
                apartment.apartment_number,
                apartment.floor_number,
                
                building.id as building_id,
                building.floor_count,
                building.apartment_count,

                building_manager.id as building_manager_user_id,
                building_manager.username as building_manager_username,
                building_manager.password as building_manager_password,
                building_manager.name as building_manager_name,
                building_manager.surname as building_manager_surname,
                building_manager.user_role as building_manager_user_role,
                
                address.id as address_id,
                address.street_number,
                address.neighborhood,
                
                street.id as street_id,
                street.name as street_name,

                city.id as city_id,
                city.name as city_name,

                country.id as country_id,
                country.name as country_name

                FROM apartment_access_requests request
                INNER JOIN users applicant ON request.applicant_id = applicant.id
                INNER JOIN apartments apartment ON request.apartment_id = apartment.id
                INNER JOIN buildings building ON apartment.building_id = building.id
                INNER JOIN users building_manager ON building.building_manager_id = building_manager.id
                INNER JOIN addresses address ON building.address_id = address.id
                INNER JOIN streets street ON address.street_id = street.id
                INNER JOIN cities city ON address.city_id = city.id
                INNER JOIN countries country ON city.country_id = country.id
                WHERE
                    request.applicant_id = @applicant_id AND
                    request.apartment_id = @apartment_id AND
                    (request.status = @status OR @status IS NULL)"
            )
            .AddParameter("applicant_id", applicantId)
            .AddParameter("apartment_id", apartmentId)
            .AddParameter("status", status);

            var requests = new List<ApartmentAccessRequest>();

            connection.ExecuteReader(
                query,
                reader => requests.Add(
                    _apartmentAccessRequestMapper.MapToEntity(reader)
                )
            );

            return requests;
        }

        public List<ApartmentAccessRequest> GetByUserAndBuilding(
            long userId,
            long buildingId,
            Status? status = null
        )
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                @"SELECT
                request.id as request_id,
                request.status,
                request.comment,
                request.creation_date,

                applicant.id as applicant_user_id,
                applicant.username as applicant_username,
                applicant.password as applicant_password,
                applicant.name as applicant_name,
                applicant.surname as applicant_surname,
                applicant.user_role as applicant_user_role,

                apartment.id as apartment_id,
                apartment.apartment_number,
                apartment.floor_number,

                building.id as building_id,
                building.floor_count,
                building.apartment_count,

                building_manager.id as building_manager_user_id,
                building_manager.username as building_manager_username,
                building_manager.password as building_manager_password,
                building_manager.name as building_manager_name,
                building_manager.surname as building_manager_surname,
                building_manager.user_role as building_manager_user_role,

                address.id as address_id,
                address.street_number,
                address.neighborhood,

                street.id as street_id,
                street.name as street_name,

                city.id as city_id,
                city.name as city_name,

                country.id as country_id,
                country.name as country_name

                FROM apartment_access_requests request
                INNER JOIN users applicant ON request.applicant_id = applicant.id
                INNER JOIN apartments apartment ON request.apartment_id = apartment.id
                INNER JOIN buildings building ON apartment.building_id = building.id
                INNER JOIN users building_manager ON building.building_manager_id = building_manager.id
                INNER JOIN addresses address ON building.address_id = address.id
                INNER JOIN streets street ON address.street_id = street.id
                INNER JOIN cities city ON address.city_id = city.id
                INNER JOIN countries country ON city.country_id = country.id
                WHERE building_manager.id = @userId AND building.id = @buildingId AND
                (request.status = @status OR @status IS NULL);"
            )
            .AddParameter("userId", userId)
            .AddParameter("buildingId", buildingId)
            .AddParameter("status", status);

            List<ApartmentAccessRequest> requests = new();

            connection.ExecuteReader(query, reader => requests.Add(
                _apartmentAccessRequestMapper.MapToEntity(reader)
            ));

            return requests;
        }

        public bool HasPendingRequest(long userId, long apartmentId)
        {
            var query = PostgresQuery.Create(
                @"SELECT EXISTS(
                    SELECT 1
                    FROM apartment_access_requests request
                    WHERE
                        request.applicant_id = @user_id AND
                        request.apartment_id = @apartment_id AND
                        request.status = 'waiting')"
            )
            .AddParameter("user_id", userId)
            .AddParameter("apartment_id", apartmentId);

            return query.ExecuteScalar(Convert.ToBoolean);
        }

        public int Update(ApartmentAccessRequest request)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                @"UPDATE apartment_access_requests
                SET status = @status, comment = @comment
                WHERE id = @request_id"
            )
            .AddParameter("status", request.Status)
            .AddParameter("request_id", request.Id)
            .AddParameter("comment", request.Comment);

            return connection.GetRowsAffectedCount(query);
        }
    }
}
