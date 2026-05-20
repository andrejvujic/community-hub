using CommunityHub.Application.Domain.Ads.TimeSlots;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.Ads
{
    public interface IAdTimeSlotReservationRepository
    {
        public void CreateMany(List<AdTimeSlotReservation> reservations);
        public void Create(AdTimeSlotReservation reservation);
    }
}
