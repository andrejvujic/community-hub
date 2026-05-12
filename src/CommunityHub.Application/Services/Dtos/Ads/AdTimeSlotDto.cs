namespace CommunityHub.Application.Services.Dtos.Ads
{
    public class AdTimeSlotDto
    {
        public long Id { get; set; }

        public long AdId { get; set; }
        public AdDto? Ad { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public AdTimeSlotReservationDto? Reservation { get; set; }

        public AdTimeSlotDto() { }

        public AdTimeSlotDto(
            long id,
            long adId,
            AdDto? ad,
            DateTime startTime,
            DateTime endTime,
            AdTimeSlotReservationDto? reservation
        )
        {
            Id = id;
            AdId = adId;
            Ad = ad;
            StartTime = startTime;
            EndTime = endTime;
            Reservation = reservation;
        }
    }
}