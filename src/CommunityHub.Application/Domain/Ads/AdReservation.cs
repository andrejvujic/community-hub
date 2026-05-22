using CommunityHub.Application.Domain.Ads.TimeSlots;
using CommunityHub.Application.Services.Dtos.Ads;

namespace CommunityHub.Application.Domain.Ads
{
    public class AdReservation
    {
        public AdDto Ad { get; }
        public List<AdTimeSlotGroup> TimeSlotGroups { get; }
        public int TimeSlotCount { get; }

        public AdReservation(AdDto ad, List<AdTimeSlotGroup> timeSlotGroups) {
            Ad = ad;
            TimeSlotGroups = timeSlotGroups;
            TimeSlotCount = timeSlotGroups.SelectMany((group) => group.TimeSlots).Count();
        }
    }
}
