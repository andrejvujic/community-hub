using CommunityHub.Application.Domain.Ads.TimeSlots;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.Ads
{
    public interface IAdTimeSlotRepository
    {
        public void Create(AdTimeSlot timeSlot);
        public void CreateMany(List<AdTimeSlot> timeSlots);

        public List<AdTimeSlot> GetTimeSlotsByAuthor(long authorId);
        public List<AdTimeSlot> GetTimeSlotsByAd(long adId);
    }
}
