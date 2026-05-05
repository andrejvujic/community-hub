using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads;

namespace CommunityHub.Application.Modules.Buildings.API.Public.Ads
{
    public interface IAdService
    {
        public Ad Publish(Ad ad);
        public Ad Archive(Ad ad);

        public List<Ad> GetByAuthorAndBuilding(long authorId, long buildingId);
        public List<Ad> GetByBuilding(long buildingId, AdStatus? status = null);

        public bool HasMatchingAds(long authorId, Ad referenceAd);
        public List<Ad> GetMatchingAds(Ad referenceAd);
    }
}
