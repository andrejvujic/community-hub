namespace CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.RepositoryInterfaces
{
    public interface IAdMatchSubscriptionRepository
    {
        public void Create(AdMatchSubscription subscription);

        public void DeleteByAdId(long adId);
        public void DeleteManyByAdId(List<long> adIds);

        public bool ExistsWithAdId(long adId);
    }
}
