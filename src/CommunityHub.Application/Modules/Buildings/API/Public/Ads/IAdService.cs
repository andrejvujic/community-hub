using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads;

namespace CommunityHub.Application.Modules.Buildings.API.Public.Ads
{
    public interface IAdService
    {
        public Ad Publish(Ad ad);

        public bool HasMatchingAds(long authorId, Ad referenceAd);
        public List<Ad> GetMatchingAds(Ad referenceAd);
    }
}
