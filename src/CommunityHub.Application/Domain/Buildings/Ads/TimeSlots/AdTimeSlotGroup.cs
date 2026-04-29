namespace CommunityHub.Application.Domain.Buildings.Ads.TimeSlots
{
    public class AdTimeSlotGroup
    {
        public DateTime Day { get; }
        public IReadOnlyList<AdTimeSlot> TimeSlots { get; }

        public AdTimeSlotGroup(DateTime day, List<AdTimeSlot> timeSlots) {
            Day = day;
            TimeSlots = timeSlots.AsReadOnly();
        }
    }
}
