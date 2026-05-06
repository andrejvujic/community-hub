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
            _adTimeSlotService.CreateMany(timeSlots);

            return publishedAd;
        }

        public Ad Archive(Ad ad)
        {
            ad.Archive();
            _adRepository.Update(ad);
            return ad;
        }

        public void ArchiveExpiredAds()
        {
            var expiredAds = _adRepository
                .GetAll()
                .Where(ad => ad.HasExpired && ad.IsActive)
                .ToList();

            foreach (var ad in expiredAds)
                ad.Archive();

            _adRepository.UpdateMany(expiredAds);
        }

        public Ad? Get(long adId)
            => _adRepository.Get(adId);

        public List<Ad> GetByBuilding(long buildingId, AdStatus? status = null)
            => _adRepository.GetByBuilding(
                buildingId,
                status: status
            );

        public List<Ad> GetByAuthorAndBuilding(long authorId, long buildingId)
            => _adRepository.GetByAuthorAndBuilding(authorId, buildingId);

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
