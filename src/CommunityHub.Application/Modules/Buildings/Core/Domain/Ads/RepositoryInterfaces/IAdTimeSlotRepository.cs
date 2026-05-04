using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.TimeSlots;

namespace CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.RepositoryInterfaces
{
    public interface IAdTimeSlotRepository
    {
        public AdTimeSlot Create(AdTimeSlot adTimeSlot);
        public AdTimeSlot? GetMatchingTimeSlotInAuthorAds(long referenceTimeSlotId, long authorId);
        public List<AdTimeSlot> GetReservableTimeSlots(long adId, long userId);
        public List<AdTimeSlot> GetReservedTimeSlots(long adId);
    }
}
