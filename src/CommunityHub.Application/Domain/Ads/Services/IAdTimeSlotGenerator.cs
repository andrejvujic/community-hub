using CommunityHub.Application.Domain.Ads;
using CommunityHub.Application.Domain.Ads.TimeSlots;

namespace CommunityHub.Application.Domain.Ads.Services
{
    public interface IAdTimeSlotGenerator
    {
        public List<AdTimeSlot> GenerateTimeSlots(
            Ad ad,
            int firstTimeSlotStartHour = 16,
            int timeSlotCount = 4
        );
    }
}
