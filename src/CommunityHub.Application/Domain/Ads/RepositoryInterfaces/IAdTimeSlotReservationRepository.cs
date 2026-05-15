using CommunityHub.Application.Domain.Ads.TimeSlots;

namespace CommunityHub.Application.Domain.Ads.RepositoryInterfaces
{
    public interface IAdTimeSlotReservationRepository
    {
        public void CreateMany(List<AdTimeSlotReservation> reservations);
        public void Create(AdTimeSlotReservation reservation);
    }
}
