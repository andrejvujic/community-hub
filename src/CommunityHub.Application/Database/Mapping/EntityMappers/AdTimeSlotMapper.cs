using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.TimeSlots;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class AdTimeSlotMapper : EntityMapper<AdTimeSlot>
    {
        private readonly AdMapper _adMapper = new();
        private readonly AdTimeSlotReservationMapper _reservationMapper = new();

        public override AdTimeSlot? MapToEntity(IDataReader reader)
        {
            if (!reader.HasColumn("time_slot_id")) return null;

            var idColumnIndex = reader.GetOrdinal("time_slot_id");
            if (reader.IsDBNull(idColumnIndex)) return null;

            var id = reader.GetInt64(idColumnIndex);

            var adId = reader.GetInt64(reader.GetOrdinal("time_slot_ad_id"));
            var ad = _adMapper.MapToEntity(reader);

            var startTime = reader.GetDateTime(reader.GetOrdinal("start_time"));
            var endTime = reader.GetDateTime(reader.GetOrdinal("end_time"));

            var reservation = _reservationMapper.MapToEntity(reader);

            return new AdTimeSlot(
                adId: adId,
                ad: ad,
                startTime: startTime,
                endTime: endTime,
                reservation: reservation,
                id: id
            );
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
