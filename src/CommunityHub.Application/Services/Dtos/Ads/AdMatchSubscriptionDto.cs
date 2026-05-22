namespace CommunityHub.Application.Services.Dtos.Ads
{
    public class AdMatchSubscriptionDto
    {
        public long Id { get; }

        public long SubscribedAdId { get; }
        public AdDto? SubscribedAd { get; }

        public AdMatchSubscriptionDto(long subscribedAdId, AdDto? subscribedAd = null, long id = 0)
        {
            Id = id;
            SubscribedAdId = subscribedAdId;
            SubscribedAd = subscribedAd;
        }
    }
}
