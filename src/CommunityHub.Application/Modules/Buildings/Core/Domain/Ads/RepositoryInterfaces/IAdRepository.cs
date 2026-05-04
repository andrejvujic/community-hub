using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads;

namespace CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.RepositoryInterfaces
{
    public interface IAdRepository
    {
        public Ad Create(Ad ad);
        public void Update(Ad ad);

        public Ad? Get(long id);
        public List<Ad> GetByBuilding(long buildingId, AdStatus? status = null);
        public List<Ad> GetByAuthorAndBuilding(long authorId, long buildingId);

        public List<Ad> GetMatchingAds(long adId);
        public bool HasMatchingAds(long authorId, Ad referenceAd);

        public int GetAdsCountByDateAndType(long buildingId, AdType type, int year, int? month);
        public List<(AdCategory, int, int)> GetAdStatsByCategory(long buildingId, int year, int? month);
        public List<(AdCategory, int)> GetActiveAdsByCategory(long buildingId);
        public int GetAdsCountByStatus(long buildingId, AdStatus status);
        public string GetMostHelpfulResident(long buildingId);

        public void ArchiveExpiredAds();
    }
}
