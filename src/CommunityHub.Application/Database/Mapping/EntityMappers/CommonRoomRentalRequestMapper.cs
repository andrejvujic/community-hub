using CommunityHub.Application.Domain.CommonRooms.RentalRequests;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class CommonRoomRentalRequestMapper : EntityMapper<CommonRoomRentalRequest>
    {
        private readonly UserMapper _userMapper = new();
        private readonly CommonRoomMapper _commonRoomMapper = new();

        public override CommonRoomRentalRequest MapToEntity(IDataReader reader)
        {
            var id = reader.GetInt64(reader.GetOrdinal("common_room_rental_request_id"));
            var status = (RentalRequestStatus)reader.GetValue(reader.GetOrdinal("rental_status"));

            var residentId = reader.GetInt64(reader.GetOrdinal("common_room_rental_request_resident_id"));
            var resident = _userMapper.MapToEntity(reader.WithPrefix("resident"));

            var commonRoomId = reader.GetInt64(reader.GetOrdinal("common_room_rental_request_common_room_id"));
            var commonRoom = _commonRoomMapper.MapToEntity(reader);

            var requestedStartDate = reader.GetDateTime(reader.GetOrdinal("requested_start_date"));
            var requestedEndDate = reader.GetDateTime(reader.GetOrdinal("requested_end_date"));

            DateTime? proposedStartDate = reader.IsDBNull(reader.GetOrdinal("proposed_start_date")) ?
                null : reader.GetDateTime(reader.GetOrdinal("proposed_start_date"));
            DateTime? proposedEndDate = reader.IsDBNull(reader.GetOrdinal("proposed_end_date")) ?
                null : reader.GetDateTime(reader.GetOrdinal("proposed_end_date"));

            DateTime? assignedDate = reader.IsDBNull(reader.GetOrdinal("assigned_date")) ?
                null : reader.GetDateTime(reader.GetOrdinal("assigned_date"));

            return new CommonRoomRentalRequest(
                commonRoomId: commonRoomId,
                commonRoom: commonRoom,
                residentId: residentId,
                resident: resident,
                requestedStartDate: requestedStartDate,
                requestedEndDate: requestedEndDate,
                proposedStartDate: proposedStartDate,
                proposedEndDate: proposedEndDate,
                status: status,
                assignedDate: assignedDate,
                id: id
            );
        }

        public override Dictionary<string, object?> MapToQueryParameters(CommonRoomRentalRequest entity)
        {
            return new Dictionary<string, object?>
            {
                { "resident_id", entity.Resident?.Id },
                { "common_room_id", entity.CommonRoom?.Id },
                { "requested_start_date", entity.RequestedStartDate },
                { "requested_end_date", entity.RequestedEndDate },
                { "proposed_start_date", entity.ProposedStartDate },
                { "proposed_end_date", entity.ProposedEndDate },
                { "rental_status", entity.Status },
                { "assigned_date", entity.AssignedDate }
            };
        }
    }
}