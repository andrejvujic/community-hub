using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads;

namespace CommunityHub.Application.Modules.Buildings.API.Public.Ads
{
    public interface IAdMatchSubscriptionService
    {
        public bool IsSubscribed(long adId);
        public void NotifyAuthorsOfSubscribedMatchingAds(Ad referenceAd);
    }
}
