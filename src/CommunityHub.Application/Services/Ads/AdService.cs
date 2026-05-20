using CommunityHub.Application.Domain.Ads;
using CommunityHub.Application.Domain.Ads.Services;
using CommunityHub.Application.Services.Ads;
using CommunityHub.Application.Services.Dtos.Ads;
using CommunityHub.Application.MappersDto.Ads;
using CommunityHub.Application.Domain.RepositoryInterfaces.Ads;
using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.Modules.Buildings.Core.Services.Ads
{
    public enum PublishmentValidationResult
    {
        Valid,
        DuplicateAdExists,
    }

    public class AdService
    {
        private readonly IAdRepository _adRepository;
        private readonly AdTimeSlotService _adTimeSlotService;
        private readonly AdTimeSlotGenerator _adTimeSlotGenerator;
        private readonly AdMapper _adMapper = new();
        public AdService(
            IAdRepository adRepository,
            IAdTimeSlotRepository adTimeSlotRepository
        )
        {
            _adRepository = adRepository;
            _adTimeSlotService = new(adTimeSlotRepository);
            _adTimeSlotGenerator = new();
        }

        public Ad Publish(Ad ad)
        {
            var publishedAd = _adRepository.Create(ad);

            var timeSlots = _adTimeSlotGenerator.GenerateTimeSlots(publishedAd);
            _adTimeSlotService.CreateMany(timeSlots);

            return publishedAd;
        }

        public PublishmentValidationResult ValidatePublishment(Ad ad)
        {
            if (HasDuplicateAds(authorId: ad.AuthorId, referenceAd: ad))
                return PublishmentValidationResult.DuplicateAdExists;

            return PublishmentValidationResult.Valid;
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

        public bool HasDuplicateAds(long authorId, Ad referenceAd)
        {
            var ads = _adRepository.GetByAuthorAndBuilding(authorId, referenceAd.BuildingId);
            return ads.Any(
                ad => ad.IsDuplicateOf(referenceAd)
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
            return ads
                .Select(ad => ad.AuthorId)
                .Distinct()
                .ToList();
        }

        public int GetAdsCountByStatus(long buildingId, AdStatus status)
            => _adRepository.GetAdsCountByStatus(buildingId, status);

        public User? GetMostHelpfulResident(long buildingId)
            => _adRepository.GetMostHelpfulResident(buildingId);

        public List<AdDto> GetAllByBuildingId(long buildingId)
        {
            var ads = _adRepository.GetByBuilding(buildingId);
            return ads.Select(_adMapper.MapToDto).ToList();
        }

        public int GetAdsCountByDateAndType(long buildingId, AdType type, int year, int? month)
            => _adRepository.GetAdsCountByDateAndType(buildingId, type, year, month);

        public List<(AdCategory Category, int OfferCount, int RequestCount)> GetAdStatsByCategory(long buildingId, int year, int? month)
            => _adRepository.GetAdStatsByCategory(buildingId, year, month);

        public List<(AdCategory Category, int ActiveAdsCount)> GetActiveAdsByCategory(long buildingId)
            => _adRepository.GetActiveAdsByCategory(buildingId);

        public List<string> GetDistinctYears()
        {
            List<string> years = _adRepository.GetDistinctYears();
            return years;
        }            
    }
}