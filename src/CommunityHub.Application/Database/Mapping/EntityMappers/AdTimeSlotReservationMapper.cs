using CommunityHub.Application.Domain.NoticeBoard;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class AdTimeSlotReservationMapper : EntityMapper<AdTimeSlotReservation>
    {
        public override AdTimeSlotReservation MapToEntity(IDataReader reader)
        {
            long id = reader.GetInt64(reader.GetOrdinal("reservation_id"));
            long firstTimeSlotId = reader.GetInt64(reader.GetOrdinal("first_time_slot_id"));
            long secondTimeSlotId = reader.GetInt64(reader.GetOrdinal("second_time_slot_id"));
            return new AdTimeSlotReservation(firstTimeSlotId, secondTimeSlotId, id);
        }

        public override Dictionary<string, object?> MapToQueryParameters(AdTimeSlotReservation entity)
            => new()
                {
                    { "first_time_slot_id", entity.FirstTimeSlotId },
                    { "second_time_slot_id", entity.SecondTimeSlotId }
                };
    }
}
