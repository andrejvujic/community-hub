using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.CommonRoomRentalRequest;
using CommunityHub.Application.Domain.NoticeBoard;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class CommonRoomRentalRequestMapper : EntityMapper<CommonRoomRentalRequest>
    {
        private readonly UserMapper _userMapper = new();

        public override CommonRoomRentalRequest MapToEntity(IDataReader reader)
        {
            User? resident = _userMapper.MapToEntity(reader.WithPrefix("resident"));
            if (resident == null)
                throw new Exception("Couldn't convert database row to entity because of missing data. Resident can't be null.");

            long id = reader.GetInt64(reader.GetOrdinal("common_room_rental_request_id"));
            long commonRoomId = reader.GetInt64(reader.GetOrdinal("common_room_id"));
            DateTime startDate = reader.GetDateTime(reader.GetOrdinal("start_date"));
            DateTime endDate = reader.GetDateTime(reader.GetOrdinal("end_date"));
            RentalStatus status = (RentalStatus)reader.GetValue(reader.GetOrdinal("rental_status"));
            DateTime? assignedDate = reader.IsDBNull(reader.GetOrdinal("assigned_date")) ? null : reader.GetDateTime(reader.GetOrdinal("assigned_date"));

            return new CommonRoomRentalRequest(
                commonRoomId,
                startDate,
                endDate,
                status,
                resident,
                assignedDate,
                id
            );
        }

        public override Dictionary<string, object?> MapToQueryParameters(CommonRoomRentalRequest entity)
        {
            return new Dictionary<string, object?>
            {
                { "resident_id", entity.Resident?.Id },
                { "common_room_id", entity.CommonRoomId },
                { "start_date", entity.StartDate },
                { "end_date", entity.EndDate },
                { "rental_status", entity.Status },
                { "assigned_date", entity.AssignedDate }
            };
        }
    }
}