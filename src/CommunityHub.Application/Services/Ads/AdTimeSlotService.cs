using CommunityHub.Application.Domain.Ads.TimeSlots;
using CommunityHub.Application.Domain.RepositoryInterfaces.Ads;

namespace CommunityHub.Application.Services.Ads
{
    public class AdTimeSlotService
    {
        private readonly IAdTimeSlotRepository _timeSlotRepository;

        public AdTimeSlotService(IAdTimeSlotRepository timeSlotRepository)
        {
            _timeSlotRepository = timeSlotRepository;
        }

        public void Create(AdTimeSlot timeSlot) =>
            _timeSlotRepository.Create(timeSlot);

        public void CreateMany(List<AdTimeSlot> timeSlots) =>
            _timeSlotRepository.CreateMany(timeSlots);

        public AdTimeSlot? FindMatchingTimeSlotForAuthor(long authorId, AdTimeSlot referenceTimeSlot)
        {
            var timeSlots = _timeSlotRepository.GetTimeSlotsByAuthor(authorId);
            return timeSlots.FirstOrDefault(
                timeSlot => timeSlot.Matches(referenceTimeSlot)
            );
        }

        public List<AdTimeSlot> GetReservableTimeSlotsForUser(long adId, long userId)
        {
            var adTimeSlots = _timeSlotRepository.GetTimeSlotsByAd(adId);
            var userTimeSlots = _timeSlotRepository.GetTimeSlotsByAuthor(userId);

            return adTimeSlots.Where(
                adTimeSlot => userTimeSlots.Any(
                    userTimeSlot => userTimeSlot.Matches(adTimeSlot)
                )
            ).ToList(); 
        }

        public List<AdTimeSlot> GetReservedTimeSlotsForAd(long adId)
        {
            var timeSlots = _timeSlotRepository.GetTimeSlotsByAd(adId);
            return timeSlots.Where(timeSlot => timeSlot.IsReserved).ToList();
        }
    }
}
