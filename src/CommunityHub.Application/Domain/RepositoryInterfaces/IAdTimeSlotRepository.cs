using CommunityHub.Application.Domain.Buildings.Ads.TimeSlots;

namespace CommunityHub.Application.Domain.RepositoryInterfaces
{
    public interface IAdTimeSlotRepository
    {
        public AdTimeSlot Create(AdTimeSlot adTimeSlot);
        public AdTimeSlot? GetMatchingTimeSlotInAuthorAds(long referenceTimeSlotId, long authorId);
        public List<AdTimeSlot> GetReservableTimeSlots(long adId, long userId);
        public List<AdTimeSlot> GetReservedTimeSlots(long adId);
    }
}
