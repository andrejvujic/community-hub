using CommunityHub.Application.Domain.Buildings.Ads.TimeSlots;

namespace CommunityHub.Application.Domain.Buildings.Ads
{
    public class AdReservation
    {
        public Ad Ad { get; }
        public List<AdTimeSlotGroup> TimeSlotGroups { get; }
        public int TimeSlotCount { get; }

        public AdReservation(Ad ad, List<AdTimeSlotGroup> timeSlotGroups) {
            Ad = ad;
            TimeSlotGroups = timeSlotGroups;
            TimeSlotCount = timeSlotGroups.SelectMany((group) => group.TimeSlots).Count();
        }
    }
}
