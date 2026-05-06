using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.TimeSlots;

namespace CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.RepositoryInterfaces
{
    public interface IAdTimeSlotReservationRepository
    {
        public void CreateMany(List<AdTimeSlotReservation> reservations);
        public void Create(AdTimeSlotReservation reservation);
    }
}
