namespace CommunityHub.Application.Services.Dtos.Ads
{
    public class AdReservationDto
    {
        public AdDto Ad { get; }
        public List<AdTimeSlotGroupDto> TimeSlotGroups { get; }
        public int TimeSlotCount { get; }

        public AdReservationDto(AdDto ad, List<AdTimeSlotGroupDto> timeSlotGroups) {
            Ad = ad;
            TimeSlotGroups = timeSlotGroups;
            TimeSlotCount = timeSlotGroups.SelectMany((group) => group.TimeSlots).Count();
        }
    }
}
