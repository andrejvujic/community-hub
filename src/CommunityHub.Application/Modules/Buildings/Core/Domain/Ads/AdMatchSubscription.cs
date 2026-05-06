namespace CommunityHub.Application.Modules.Buildings.Core.Domain.Ads
{
    public class AdMatchSubscription
    {
        public long Id { get; }
        
        public long SubscribedAdId { get; }
        public Ad? SubscribedAd { get; }

        public AdMatchSubscription(long subscribedAdId, Ad? subscribedAd = null, long id = 0)
        {
            Id = id;
            SubscribedAdId = subscribedAdId;
            SubscribedAd = subscribedAd;
        }
    }
}
