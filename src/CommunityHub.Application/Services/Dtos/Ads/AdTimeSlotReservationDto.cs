namespace CommunityHub.Application.Services.Dtos.Ads
{
    public class AdTimeSlotReservationDto
    {
        public long Id { get; set; }

        public long OfferTimeSlotId { get; set; }
        public long RequestTimeSlotId { get; set; }

        public AdDto? OfferAd { get; set; }
        public AdDto? RequestAd { get; set; }

        public AdTimeSlotReservationDto() { }

        public AdTimeSlotReservationDto(
            long id,
            long offerTimeSlotId,
            long requestTimeSlotId,
            AdDto? offerAd,
            AdDto? requestAd
        )
        {
            Id = id;
            OfferTimeSlotId = offerTimeSlotId;
            RequestTimeSlotId = requestTimeSlotId;
            OfferAd = offerAd;
            RequestAd = requestAd;
        }
    }
}