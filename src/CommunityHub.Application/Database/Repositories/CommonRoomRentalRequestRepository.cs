using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.CommonRoomRentalRequest;

namespace CommunityHub.Application.Database.Repositories
{
    public class CommonRoomRentalRequestRepository
    {
        private readonly CommonRoomRentalRequestMapper _commonRoomRentalRequestMapper = new();

        public void Create(CommonRoomRentalRequest request)
            => PostgresQuery.Create(
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
                .ExtendParameters(_commonRoomRentalRequestMapper.MapToQueryParameters(request))
                .Execute();

        public void Delete(long id)
            =>  PostgresQuery.Create(
                    @"DELETE FROM common_room_rental_requests
                      WHERE id = @id;"
                )
                .AddParameter("id", id)
                .Execute();

        public void Update(CommonRoomRentalRequest request)
            => PostgresQuery.Create(
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
                .AddParameter("id", request.Id)
                .Execute();

        public List<DateTime> GetReservedDates(long commonRoomId, long buildingId)
        {
            var query = PostgresQuery.Create(
                 @"SELECT common_room_rental_request.id as common_room_rental_request_id, 
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

            query.ExecuteReader(reader =>
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
            var query = PostgresQuery.Create(
                 @"SELECT common_room_rental_request.id as common_room_rental_request_id, 
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
                   WHERE common_room.building_id = @building_id;"
            )
            .AddParameter("building_id", buildingId);

            var rentalRequests = new List<CommonRoomRentalRequest>();
            query.ExecuteReader(reader =>
            {
                var commonRoomRentalRequest = _commonRoomRentalRequestMapper.MapToEntity(reader);
                rentalRequests.Add(commonRoomRentalRequest);
            });

            return rentalRequests;
        }

        public int UpdateStatusAndAssignedDate(long requestId, RentalRequestStatus newStatus, DateTime? assignedDate)
        {
            var query = PostgresQuery.Create(
                @"UPDATE common_room_rental_requests
                  SET rental_status = @new_status, 
                      assigned_date = @assigned_date
                  WHERE id = @request_id;"
            )
            .AddParameter("new_status", newStatus)
            .AddParameter("assigned_date", assignedDate)
            .AddParameter("request_id", requestId);

            return query.GetRowsAffectedCount();
        }

        public int UpdateStatusAndRange(long requestId, RentalRequestStatus newStatus, DateTime proposedStartDate, DateTime proposedEndDate)
        {
            var query = PostgresQuery.Create(
                @"UPDATE common_room_rental_requests
                  SET rental_status = @new_status, 
                      proposed_start_date = @proposed_start_date,
                      proposed_end_date = @proposed_end_date
                  WHERE id = @request_id;"
            )
            .AddParameter("new_status", newStatus)
            .AddParameter("proposed_start_date", proposedStartDate)
            .AddParameter("proposed_end_date", proposedEndDate)
            .AddParameter("request_id", requestId);

            return query.GetRowsAffectedCount();
        }

        public List<CommonRoomRentalRequest> GetByResidentAndCommonRoom(long residentId, long commonRoomId)
        {
            var query = PostgresQuery.Create(
                 @"SELECT common_room_rental_request.id as common_room_rental_request_id, 
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

            query.ExecuteReader(reader => requests.Add(
                    _commonRoomRentalRequestMapper.MapToEntity(reader)
                )
            );

            return requests;
        }

        public bool HasPendingRequest(long userId, long commonRoomId)
        {
            var query = PostgresQuery.Create(
                @"SELECT EXISTS(
                    SELECT 1
                    FROM common_room_rental_requests
                    WHERE
                        resident_id = @user_id AND
                        common_room_id = @common_room_id AND
                        (rental_status = 'waiting' OR rental_status = 'waiting_date_change'))"
            )
            .AddParameter("user_id", userId)
            .AddParameter("common_room_id", commonRoomId);

            return query.ExecuteScalar(Convert.ToBoolean);
        }
    }
}   