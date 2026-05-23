using CommunityHub.Application.Domain.Ads;
using CommunityHub.Application.Domain.Ads.Services;
using CommunityHub.Application.Services.Ads;
using CommunityHub.Application.Services.Dtos.Ads;
using CommunityHub.Application.MappersDto.Ads;
using CommunityHub.Application.Domain.RepositoryInterfaces.Ads;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.Services;
using CommunityHub.Application.MappersDto.Users;
using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.Modules.Buildings.Core.Services.Ads
{
    public enum PublishmentValidationResult
    {
        Valid,
        DuplicateAdExists,
    }

    public class AdService : BaseService<Ad, AdDto>
    {
        private readonly IAdRepository _adRepository;
        private readonly AdTimeSlotService _adTimeSlotService;
        private readonly AdTimeSlotGenerator _adTimeSlotGenerator;
        private readonly AdMapper _adMapper = new();
        private readonly UserMapper _userMapper = new();
        public AdService(
            IAdRepository adRepository,
            IAdTimeSlotRepository adTimeSlotRepository
        ) : base(mapper: new AdMapper())
        {
            _adRepository = adRepository;
            _adTimeSlotService = new(adTimeSlotRepository);
            _adTimeSlotGenerator = new();
        }

        public AdDto Publish(AdDto ad)
        {
            var publishedAd = _adRepository.Create(
                MapToEntity(ad)
            );

            var timeSlots = _adTimeSlotGenerator.GenerateTimeSlots(publishedAd);
            _adTimeSlotService.CreateMany(timeSlots);

            return MapToDto(publishedAd);
        }

        public PublishmentValidationResult ValidatePublishment(AdDto ad)
        {
            if (HasDuplicateAds(authorId: ad.AuthorId, referenceAd: ad))
                return PublishmentValidationResult.DuplicateAdExists;

            return PublishmentValidationResult.Valid;
        }

        public AdDto Archive(AdDto ad)
        {
            var targetAd = MapToEntity(ad);
            targetAd.Archive();

            _adRepository.Update(targetAd);

            return MapToDto(targetAd);
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

        public AdDto? Get(long adId)
        {
            var ad = _adRepository.Get(adId);
            if (ad is null) return null;
            return MapToDto(ad);
        }

        public List<AdDto> GetByBuilding(long buildingId, AdStatus? status = null)
            => MapToDto(
                _adRepository.GetByBuilding(
                    buildingId,
                    status: status
                )
            );

        public List<AdDto> GetByAuthorAndBuilding(long authorId, long buildingId)
            => MapToDto(
                _adRepository.GetByAuthorAndBuilding(authorId, buildingId)
            );

        public bool HasDuplicateAds(long authorId, AdDto referenceAd)
        {
            var ads = _adRepository.GetByAuthorAndBuilding(authorId, referenceAd.BuildingId);
            return ads.Any(
                ad => ad.IsDuplicateOf(
                    MapToEntity(referenceAd)
                )
            );
        }

        public List<AdDto> GetMatchingAds(AdDto referenceAd)
        {
            var ads = _adRepository.GetByBuilding(referenceAd.BuildingId);

            var matchingAds = ads.Where(
                ad => ad.Matches(
                    MapToEntity(referenceAd)
                )
            ).ToList();

            return MapToDto(matchingAds);
        }

        public List<long> GetAuthorIdsOfMatchingAds(AdDto referenceAd)
        {
            var ads = GetMatchingAds(referenceAd);
            return ads
                .Select(ad => ad.AuthorId)
                .Distinct()
                .ToList();
        }

        public int GetAdsCountByStatus(long buildingId, AdStatus status)
            => _adRepository.GetAdsCountByStatus(buildingId, status);

        public UserDto? GetMostHelpfulResident(long buildingId)
        {
            User? user = _adRepository.GetMostHelpfulResident(buildingId);
            if (user is null) return null;
            return _userMapper.MapToDto(user);
        }

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