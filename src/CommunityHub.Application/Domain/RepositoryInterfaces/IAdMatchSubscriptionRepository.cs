using CommunityHub.Application.Domain.Buildings.Ads;

namespace CommunityHub.Application.Domain.RepositoryInterfaces
{
    public interface IAdMatchSubscriptionRepository
    {
        public void Create(AdMatchSubscription subscription);
        public void DeleteByAdId(long adId);
        public bool IsSubscribed(long adId);
    }
}
