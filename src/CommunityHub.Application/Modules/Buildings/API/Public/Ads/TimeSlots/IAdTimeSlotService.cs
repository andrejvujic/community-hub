using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.TimeSlots;

namespace CommunityHub.Application.Modules.Buildings.API.Public.Ads.TimeSlots
{
    public interface IAdTimeSlotService
    {
        public void Create(AdTimeSlot timeSlot);
        public void CreateMany(List<AdTimeSlot> timeSlots);

        public List<AdTimeSlot> GetReservedTimeSlotsForAd(long adId);
        public List<AdTimeSlot> GetReservableTimeSlotsForUser(long adId, long userId);

        public AdTimeSlot? FindMatchingTimeSlotForAuthor(long authorId, AdTimeSlot referenceTimeSlot);
    }
}
