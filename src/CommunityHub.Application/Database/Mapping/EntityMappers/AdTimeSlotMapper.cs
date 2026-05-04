using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.TimeSlots;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class AdTimeSlotMapper : EntityMapper<AdTimeSlot>
    {
        private readonly AdTimeSlotReservationMapper _reservationMapper = new();

        public override AdTimeSlot MapToEntity(IDataReader reader)
        {
            AdTimeSlotReservation? reservation = _reservationMapper.MapToEntity(reader);

            long id = reader.GetInt64(reader.GetOrdinal("time_slot_id"));
            long adId = reader.GetInt64(reader.GetOrdinal("time_slot_ad_id"));
            DateTime startTime = reader.GetDateTime(reader.GetOrdinal("start_time"));
            DateTime endTime = reader.GetDateTime(reader.GetOrdinal("end_time"));

            return new AdTimeSlot(adId, startTime, endTime, reservation, id);
        }

        public override Dictionary<string, object?> MapToQueryParameters(AdTimeSlot entity)
        {
            return new Dictionary<string, object?>
            {
                { "ad_id", entity.AdId  },
                { "start_time", entity.StartTime },
                { "end_time", entity.EndTime }
            };
        }
    }
}
