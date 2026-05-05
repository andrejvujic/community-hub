using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.TimeSlots;

namespace CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.RepositoryInterfaces
{
    public interface IAdTimeSlotRepository
    {
        public AdTimeSlot Create(AdTimeSlot timeSlot);

        public List<AdTimeSlot> GetTimeSlotsByAuthor(long authorId);
        public List<AdTimeSlot> GetTimeSlotsByAd(long adId);
    }
}
