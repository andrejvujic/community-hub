using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads;

namespace CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.RepositoryInterfaces
{
    public interface IAdMatchSubscriptionRepository
    {
        public void Create(AdMatchSubscription subscription);
        public void DeleteByAdId(long adId);
        public bool IsSubscribed(long adId);
    }
}
