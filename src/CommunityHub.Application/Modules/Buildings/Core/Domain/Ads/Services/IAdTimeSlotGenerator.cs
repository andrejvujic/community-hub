using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.TimeSlots;

namespace CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.Services
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
