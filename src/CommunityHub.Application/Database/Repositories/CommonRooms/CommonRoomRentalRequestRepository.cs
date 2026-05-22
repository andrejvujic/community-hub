using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.CommonRooms.RentalRequests;
using CommunityHub.Application.Domain.RepositoryInterfaces.CommonRooms;

namespace CommunityHub.Application.Database.Repositories.CommonRooms
{
    public class CommonRoomRentalRequestRepository : ICommonRoomRentalRequestRepository
    {
        private readonly CommonRoomRentalRequestMapper _commonRoomRentalRequestMapper = new();

        public void Create(CommonRoomRentalRequest request)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                @"INSERT INTO common_room_rental_requests
                        (common_room_id,
                        resident_id,
                        requested_start_date,
                        requested_end_date,
                        proposed_start_date,
                        proposed_end_date,
                        rental_status,
                        assigned_date)
                    VALUES
                        (@common_room_id,
                        @resident_id,
                        @requested_start_date,
                        @requested_end_date,
                        @proposed_start_date,
                        @proposed_end_date,
                        @rental_status,
                        @assigned_date);"
            )
            .ExtendParameters(
                _commonRoomRentalRequestMapper.MapToQueryParameters(request)
            );

            connection.Execute(query);
        }

        public void Delete(long id)
        {
            using var connection = DatabaseConnection.Create();

            connection.Execute(
                DatabaseQuery.Create(
                    @"DELETE FROM common_room_rental_requests WHERE id = @id;"
                )
                .AddParameter("id", id)
            );
        }

        public void Update(CommonRoomRentalRequest request)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                @"UPDATE common_room_rental_requests SET
                    common_room_id = @common_room_id,
                    resident_id = @resident_id,
                    requested_start_date = @requested_start_date,
                    requested_end_date = @requested_end_date,
                    proposed_start_date = @proposed_start_date,
                    proposed_end_date = @proposed_end_date,
                    rental_status = @rental_status,
                    assigned_date = @assigned_date
                WHERE id = @id;"
            )
            .ExtendParameters(_commonRoomRentalRequestMapper.MapToQueryParameters(request))
            .AddParameter("id", request.Id);

            connection.Execute(query);
        }

        public List<DateTime> GetReservedDates(long commonRoomId, long buildingId)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                 @"SELECT common_room_rental_request.id as common_room_rental_request_id, 
                          common_room_rental_request.common_room_id AS common_room_rental_request_common_room_id,
                          common_room_rental_request.resident_id AS common_room_rental_request_resident_id,
                          common_room_rental_request.requested_start_date, 
                          common_room_rental_request.requested_end_date, 
                          common_room_rental_request.proposed_start_date,
                          common_room_rental_request.proposed_end_date,
                          common_room_rental_request.rental_status,
                          common_room_rental_request.assigned_date,

                          resident.id AS resident_user_id,
                          resident.username AS resident_username,
                          resident.password AS resident_password,
                          resident.name AS resident_name,
                          resident.surname AS resident_surname,
                          resident.user_role AS resident_user_role,

                          common_room.id AS common_room_id,
                          common_room.building_id,
                          common_room.name,
                          common_room.description,
                          common_room.floor_number,
                          common_room.rental_type

                   FROM common_room_rental_requests common_room_rental_request
                   INNER JOIN common_rooms common_room ON
                   common_room_rental_request.common_room_id = common_room.id
                   INNER JOIN users resident ON
                   common_room_rental_request.resident_id = resident.id
                   WHERE common_room.building_id = @building_id
                   AND common_room_rental_request.rental_status = 'approved'
                   AND common_room.id = @common_room_id;"
            )
            .AddParameter("building_id", buildingId)
            .AddParameter("common_room_id", commonRoomId);

            var reservedDates = new List<DateTime>();

            connection.ExecuteReader(query, reader => 
            {
                var commonRoomRentalRequest = _commonRoomRentalRequestMapper.MapToEntity(reader);

                if (commonRoomRentalRequest.AssignedDate != null)
                    reservedDates.Add(commonRoomRentalRequest.AssignedDate.Value);
                else
                {
                    var currentDate = commonRoomRentalRequest.RequestedStartDate;
                    while (currentDate <= commonRoomRentalRequest.RequestedEndDate)
                    {
                        reservedDates.Add(currentDate);
                        currentDate = currentDate.AddDays(1);
                    }
                }
            });

            return reservedDates;
        }

        public List<CommonRoomRentalRequest> GetAllByBuildingId(long buildingId)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                 @"SELECT common_room_rental_request.id as common_room_rental_request_id, 
                        common_room_rental_request.common_room_id AS common_room_rental_request_common_room_id,
                        common_room_rental_request.resident_id AS common_room_rental_request_resident_id,
                        common_room_rental_request.requested_start_date, 
                        common_room_rental_request.requested_end_date, 
                        common_room_rental_request.proposed_start_date,
                        common_room_rental_request.proposed_end_date,
                        common_room_rental_request.rental_status,
                        common_room_rental_request.assigned_date,
                        resident.id AS resident_user_id,
                        resident.username AS resident_username,
                        resident.password AS resident_password,
                        resident.name AS resident_name,
                        resident.surname AS resident_surname,
                        resident.user_role AS resident_user_role,
                        common_room.id AS common_room_id,
                        common_room.building_id,
                        common_room.name,
                        common_room.description,
                        common_room.floor_number,
                        common_room.rental_type
                   FROM common_room_rental_requests common_room_rental_request
                   INNER JOIN common_rooms common_room ON
                   common_room_rental_request.common_room_id = common_room.id
                   INNER JOIN users resident ON
                   common_room_rental_request.resident_id = resident.id
                   WHERE common_room.building_id = @building_id
                   ORDER BY common_room_rental_request_id;"
            )
            .AddParameter("building_id", buildingId);

            var rentalRequests = new List<CommonRoomRentalRequest>();
            connection.ExecuteReader(query, reader =>
            {
                var commonRoomRentalRequest = _commonRoomRentalRequestMapper.MapToEntity(reader);
                rentalRequests.Add(commonRoomRentalRequest);
            });

            return rentalRequests;
        }

        public List<CommonRoomRentalRequest> GetByResidentAndCommonRoom(long residentId, long commonRoomId)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                 @"SELECT common_room_rental_request.id as common_room_rental_request_id, 
                        common_room_rental_request.common_room_id AS common_room_rental_request_common_room_id,
                        common_room_rental_request.resident_id AS common_room_rental_request_resident_id,
                        common_room_rental_request.requested_start_date, 
                        common_room_rental_request.requested_end_date, 
                        common_room_rental_request.proposed_start_date,
                        common_room_rental_request.proposed_end_date,
                        common_room_rental_request.rental_status AS rental_status,
                        common_room_rental_request.assigned_date,
                        resident.id AS resident_user_id,
                        resident.username AS resident_username,
                        resident.password AS resident_password,
                        resident.name AS resident_name,
                        resident.surname AS resident_surname,
                        resident.user_role AS resident_user_role,
                        common_room.id AS common_room_id,
                        common_room.building_id,
                        common_room.name,
                        common_room.description,
                        common_room.floor_number,
                        common_room.rental_type

                   FROM common_room_rental_requests common_room_rental_request

                   INNER JOIN common_rooms common_room ON
                        common_room_rental_request.common_room_id = common_room.id

                   INNER JOIN users resident ON
                        common_room_rental_request.resident_id = resident.id
                   
                   WHERE resident.id = @resident_id
                        AND common_room.id = @common_room_id

                   ORDER BY common_room_rental_request_id DESC"
            )
            .AddParameter("resident_id", residentId)
            .AddParameter("common_room_id", commonRoomId);

            var requests = new List<CommonRoomRentalRequest>();

            connection.ExecuteReader(
                query,
                reader => requests.Add(
                    _commonRoomRentalRequestMapper.MapToEntity(reader)
                )
            );

            return requests;
        }
    }
}   