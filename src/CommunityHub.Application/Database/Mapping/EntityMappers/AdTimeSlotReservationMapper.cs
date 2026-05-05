using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.TimeSlots;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class AdTimeSlotReservationMapper : EntityMapper<AdTimeSlotReservation>
    {
        private readonly AdMapper _adMapper = new();
        public override AdTimeSlotReservation? MapToEntity(IDataReader reader)
        {
            if (!reader.HasColumn("reservation_id")) return null;

            var idColumnIndex = reader.GetOrdinal("reservation_id");
            if (reader.IsDBNull(idColumnIndex))
                return null;

            var id = reader.GetInt64(reader.GetOrdinal("reservation_id"));

            var offerAd = _adMapper.MapToEntity(reader.WithPrefix("offer_ad"));

            var offerTimeSlotId = reader.GetInt64(
                reader.GetOrdinal("offer_ad_time_slot_id")
            );

            var requestAd = _adMapper.MapToEntity(reader.WithPrefix("request_ad"));

            var requestTimeSlotId = reader.GetInt64(
                reader.GetOrdinal("request_ad_time_slot_id")
            );

            return new AdTimeSlotReservation(
                id: id,
                offerAd: offerAd,
                offerTimeSlotId: offerTimeSlotId,
                requestAd: requestAd,
                requestTimeSlotId: requestTimeSlotId
            );
        }

        public override Dictionary<string, object?> MapToQueryParameters(AdTimeSlotReservation entity)
            => new()
                {
                    { "request_ad_time_slot_id", entity.RequestTimeSlotId },
                    { "offer_ad_time_slot_id", entity.OfferTimeSlotId }
                };
    }
}
