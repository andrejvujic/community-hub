using CommunityHub.Application.Services.Dtos.Ads;

namespace CommunityHub.Application.Domain.Ads.TimeSlots
{
    public class AdTimeSlotGroup
    {
        public DateTime Day { get; }
        public IReadOnlyList<AdTimeSlotDto> TimeSlots { get; }

        public AdTimeSlotGroup(DateTime day, List<AdTimeSlotDto> timeSlots) {
            Day = day;
            TimeSlots = timeSlots.AsReadOnly();
        }
    }
}
