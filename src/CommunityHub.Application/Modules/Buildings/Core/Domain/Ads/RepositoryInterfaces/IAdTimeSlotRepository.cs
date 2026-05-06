using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.TimeSlots;

namespace CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.RepositoryInterfaces
{
    public interface IAdTimeSlotRepository
    {
        public void Create(AdTimeSlot timeSlot);
        public void CreateMany(List<AdTimeSlot> timeSlots);

        public List<AdTimeSlot> GetTimeSlotsByAuthor(long authorId);
        public List<AdTimeSlot> GetTimeSlotsByAd(long adId);
    }
}
