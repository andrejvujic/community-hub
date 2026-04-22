
using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.CommonRoomRentalRequest;
using System.Diagnostics;

namespace CommunityHub.Application.Database.Repositories
{
    public class CommonRoomRentalRequestRepository
    {
        private readonly CommonRoomRentalRequestMapper _commonRoomRentalRequestMapper = new();
        public List<DateTime> GetApprovedDates(long commonRoomId, long buildingId)
        {
            var query = PostgresQuery.Create(
                 @"SELECT common_room_rental_request.id as common_room_rental_request_id, 
                          common_room_rental_request.start_date, 
                          common_room_rental_request.end_date, 
                          common_room_rental_request.rental_statuses AS rental_status,
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
                   AND common_room_rental_request.rental_statuses = 'approved'
                   AND common_room.id = @common_room_id;"
            )
            .AddParameter("building_id", buildingId)
            .AddParameter("common_room_id", commonRoomId);

            var approvedDates = new List<DateTime>();

            query.ExecuteReader(reader =>
            {
                var commonRoomRentalRequest = _commonRoomRentalRequestMapper.MapToEntity(reader);

                if (commonRoomRentalRequest.AssignedDate != null)
                    approvedDates.Add(commonRoomRentalRequest.AssignedDate.Value);
                else
                {
                    var currentDate = commonRoomRentalRequest.StartDate;
                    while (currentDate <= commonRoomRentalRequest.EndDate)
                    {
                        approvedDates.Add(currentDate);
                        currentDate = currentDate.AddDays(1);
                    }
                }
            });

            return approvedDates;
        }

        public List<CommonRoomRentalRequest> GetAllByBuildingId(long buildingId)
        {
            var query = PostgresQuery.Create(
                 @"SELECT common_room_rental_request.id as common_room_rental_request_id, 
                          common_room_rental_request.start_date, 
                          common_room_rental_request.end_date, 
                          common_room_rental_request.rental_statuses AS rental_status,
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
    }
}