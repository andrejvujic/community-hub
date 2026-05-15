using CommunityHub.Application.Domain.Ads.TimeSlots;

namespace CommunityHub.Application.Domain.Ads.Services
{
    public interface IAdTimeSlotReservationBuilder
    {
        public List<AdTimeSlotReservation>? CreateReservations(long userId, List<AdTimeSlot> timeSlots); 
    }
}
