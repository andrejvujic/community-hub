using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.TimeSlots;

namespace CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.Services
{
    public interface IAdTimeSlotReservationBuilder
    {
        public List<AdTimeSlotReservation>? CreateReservations(long userId, List<AdTimeSlot> timeSlots); 
    }
}
