using CommunityHub.Application.Domain.Buildings.Ads.TimeSlots;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class AdTimeSlotReservationMapper : EntityMapper<AdTimeSlotReservation>
    {
        private readonly AdMapper _adMapper = new();

        public override AdTimeSlotReservation? MapToEntity(IDataReader reader)
        {
            if (!reader.HasColumn("reservation_id")) return null;

            int idColumnIndex = reader.GetOrdinal("reservation_id");
            if (reader.IsDBNull(idColumnIndex))
                return null;

            var ad1 = _adMapper.MapToEntity(reader.WithPrefix("ad_1"));
            var ad2 = _adMapper.MapToEntity(reader.WithPrefix("ad_2"));

            var offerAd = ad1.IsOffer ? ad1 : ad2;
            var requestAd = ad1.IsRequest ? ad1 : ad2;

            long id = reader.GetInt64(reader.GetOrdinal("reservation_id"));
            long offerTimeSlotId = reader.GetInt64(reader.GetOrdinal("offer_ad_time_slot_id"));
            long requestTimeSlotId = reader.GetInt64(reader.GetOrdinal("request_ad_time_slot_id"));

            return new AdTimeSlotReservation(
                offerTimeSlotId,
                requestTimeSlotId,
                offerAd,
                requestAd,
                id
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
