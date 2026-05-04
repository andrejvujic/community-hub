namespace CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.RepositoryInterfaces
{
    public interface IAdTimeSlotReservationRepository
    {
        public void Create(long offerAdTimeSlotId, long requestAdTimeSlotId);
    }
}
