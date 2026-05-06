using CommunityHub.Application.Modules.Buildings.API.Public.Ads.TimeSlots;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.RepositoryInterfaces;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.TimeSlots;

namespace CommunityHub.Application.Modules.Buildings.Core.Services.Ads
{
    public class AdTimeSlotService : IAdTimeSlotService
    {
        private readonly IAdTimeSlotRepository _adTimeSlotRepository;

        public AdTimeSlotService(IAdTimeSlotRepository adTimeSlotRepository)
        {
            _adTimeSlotRepository = adTimeSlotRepository;
        }

        public void Create(AdTimeSlot timeSlot)
        {
            _adTimeSlotRepository.Create(timeSlot);
        }

        public AdTimeSlot? FindMatchingTimeSlotForAuthor(long authorId, AdTimeSlot referenceTimeSlot)
        {
            var timeSlots = _adTimeSlotRepository.GetTimeSlotsByAuthor(authorId);
            return timeSlots.FirstOrDefault(
                timeSlot => timeSlot.Matches(referenceTimeSlot)
            );
        }

        public List<AdTimeSlot> GetReservableTimeSlotsForUser(long adId, long userId)
        {
            var adTimeSlots = _adTimeSlotRepository.GetTimeSlotsByAd(adId);
            var userTimeSlots = _adTimeSlotRepository.GetTimeSlotsByAuthor(userId);

            return adTimeSlots.Where(
                adTimeSlot => userTimeSlots.Any(
                    userTimeSlot => userTimeSlot.Matches(adTimeSlot)
                )
            ).ToList(); 
        }

        public List<AdTimeSlot> GetReservedTimeSlotsForAd(long adId)
        {
            var timeSlots = _adTimeSlotRepository.GetTimeSlotsByAd(adId);
            return timeSlots.Where(timeSlot => timeSlot.IsReserved).ToList();
        }
    }
}
