using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.TimeSlots;

namespace CommunityHub.Application.Modules.Buildings.API.Public.Ads.TimeSlots
{
    public interface IAdTimeSlotReservationService
    {
        public List<AdTimeSlotReservation>? Reserve(long userId, List<AdTimeSlot> timeSlots);
    }
}
