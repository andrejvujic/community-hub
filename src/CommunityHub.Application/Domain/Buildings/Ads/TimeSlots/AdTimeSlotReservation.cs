using CommunityHub.Application.Domain.Buildings.Ads;

namespace CommunityHub.Application.Domain.Buildings.Ads.TimeSlots
{
    public class AdTimeSlotReservation
    {
        public long Id { get; }

        public long OfferTimeSlotId { get;  }
        public long RequestTimeSlotId { get; }

        public Ad OfferAd { get; }
        public Ad RequestAd { get; }

        public AdTimeSlotReservation(
            long offerTimeSlotId,
            long requestTimeSlotId,
            Ad offerAd,
            Ad requestAd,
            long id = 0
        )
        {  
            RequestTimeSlotId = requestTimeSlotId;
            OfferTimeSlotId = offerTimeSlotId;
            RequestAd = requestAd;
            OfferAd = offerAd;
            Id = id;
        }

        public AdTimeSlotReservation CopyWith(
            long? id = null,
            long? offerTimeSlotId = null,
            long? requestTimeSlotId = null,
            Ad? offerAd = null,
            Ad? requestAd = null
        ) => new AdTimeSlotReservation(
                offerTimeSlotId ?? OfferTimeSlotId,
                requestTimeSlotId ?? RequestTimeSlotId,
                offerAd ?? OfferAd,
                requestAd ?? RequestAd,
                id ?? Id
            );
    }
}
