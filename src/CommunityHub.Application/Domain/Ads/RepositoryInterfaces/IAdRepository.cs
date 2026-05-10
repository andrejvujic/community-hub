using CommunityHub.Application.Domain.Ads;

namespace CommunityHub.Application.Domain.Ads.RepositoryInterfaces
{
    public interface IAdRepository
    {
        public Ad Create(Ad ad);

        public void Update(Ad ad);
        public void UpdateMany(List<Ad> ad);

        public Ad? Get(long id);
        public List<Ad> GetAll();
        public List<Ad> GetByBuilding(long buildingId, AdStatus? status = null);
        public List<Ad> GetByAuthorAndBuilding(long authorId, long buildingId);

        public int GetAdsCountByDateAndType(long buildingId, AdType type, int year, int? month);
        public List<(AdCategory, int, int)> GetAdStatsByCategory(long buildingId, int year, int? month);
        public List<(AdCategory, int)> GetActiveAdsByCategory(long buildingId);
        public int GetAdsCountByStatus(long buildingId, AdStatus status);
        public string GetMostHelpfulResident(long buildingId);
    }
}
