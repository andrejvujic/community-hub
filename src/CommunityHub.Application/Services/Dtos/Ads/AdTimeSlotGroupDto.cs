namespace CommunityHub.Application.Services.Dtos.Ads
{
    public class AdTimeSlotGroupDto
    {
        public DateTime Day { get; }
        public IReadOnlyList<AdTimeSlotDto> TimeSlots { get; }

        public AdTimeSlotGroupDto(DateTime day, List<AdTimeSlotDto> timeSlots) {
            Day = day;
            TimeSlots = timeSlots.AsReadOnly();
        }
    }
}
