using CommunityHub.Application.Domain.Buildings.CommonRooms;
using CommunityHub.Application.Domain.Buildings.CommonRooms.RentalRequests;
using CommunityHub.Application.Modules.Shared.Core.Domain.Users;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class CommonRoomRentalRequestMapper : EntityMapper<CommonRoomRentalRequest>
    {
        private readonly UserMapper _userMapper = new();
        private readonly CommonRoomMapper _commonRoomMapper = new();

        public override CommonRoomRentalRequest MapToEntity(IDataReader reader)
        {
            User? resident = _userMapper.MapToEntity(reader.WithPrefix("resident"));
            if (resident == null)
                throw new Exception("CommonRoomRentalRequest.Resident can't be null");

            CommonRoom commonRoom = _commonRoomMapper.MapToEntity(reader);


            long id = reader.GetInt64(reader.GetOrdinal("common_room_rental_request_id"));
            RentalRequestStatus status = (RentalRequestStatus)reader.GetValue(reader.GetOrdinal("rental_status"));

            DateTime requestedStartDate = reader.GetDateTime(reader.GetOrdinal("requested_start_date"));

            DateTime requestedEndDate = reader.GetDateTime(reader.GetOrdinal("requested_end_date"));

            DateTime? proposedStartDate = reader.IsDBNull(reader.GetOrdinal("proposed_start_date")) ?
                null : reader.GetDateTime(reader.GetOrdinal("proposed_start_date"));

            DateTime? proposedEndDate = reader.IsDBNull(reader.GetOrdinal("proposed_end_date")) ?
                null : reader.GetDateTime(reader.GetOrdinal("proposed_end_date"));

            DateTime? assignedDate = reader.IsDBNull(reader.GetOrdinal("assigned_date")) ?
                null : reader.GetDateTime(reader.GetOrdinal("assigned_date"));

            return new CommonRoomRentalRequest(
                commonRoom: commonRoom,
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