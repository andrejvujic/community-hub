namespace CommunityHub.Application.Domain.RepositoryInterfaces
{
    public interface IAdTimeSlotReservationRepository
    {
        public void Create(long offerAdTimeSlotId, long requestAdTimeSlotId);
    }
}
