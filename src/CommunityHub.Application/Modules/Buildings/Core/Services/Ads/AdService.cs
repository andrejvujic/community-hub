using CommunityHub.Application.Modules.Buildings.API.Public.Ads;
using CommunityHub.Application.Modules.Buildings.API.Public.Ads.TimeSlots;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.RepositoryInterfaces;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.Services;

namespace CommunityHub.Application.Modules.Buildings.Core.Services.Ads
{
    public class AdService : IAdService
    {
        private readonly IAdRepository _adRepository;
        private readonly IAdTimeSlotService _adTimeSlotService;
        private readonly IAdTimeSlotGenerator _adTimeSlotGenerator;

        public AdService(
            IAdRepository adRepository,
            IAdTimeSlotService adTimeSlotService,
            IAdTimeSlotGenerator adTimeSlotGenerator
        ) {
            _adRepository = adRepository;
            _adTimeSlotService = adTimeSlotService;
            _adTimeSlotGenerator = adTimeSlotGenerator;
        }

        public Ad Publish(Ad ad)
        {
            var publishedAd = _adRepository.Create(ad);

            var timeSlots = _adTimeSlotGenerator.GenerateTimeSlots(publishedAd);
            foreach (var timeSlot in timeSlots)
                _adTimeSlotService.Create(timeSlot);

            return publishedAd;
        }

        public bool HasMatchingAds(long authorId, Ad referenceAd)
        {
            var ads = _adRepository.GetByAuthorAndBuilding(authorId, referenceAd.BuildingId);
            return ads.Any(
                ad => ad.Matches(referenceAd)
            );
        }

        public List<Ad> GetMatchingAds(Ad referenceAd)
        {
            var ads = _adRepository.GetByBuilding(referenceAd.BuildingId);
            return ads.Where(
                ad => ad.Matches(referenceAd)
            ).ToList();
        }

        public List<long> GetAuthorIdsOfMatchingAds(Ad referenceAd)
        {
            var ads = GetMatchingAds(referenceAd);
            return ads.Select(ad => ad.Author.Id).ToList();
        }
    }
}
