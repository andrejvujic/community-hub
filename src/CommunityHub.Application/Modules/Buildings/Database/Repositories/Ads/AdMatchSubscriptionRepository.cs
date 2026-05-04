using CommunityHub.Application.Database;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.RepositoryInterfaces;

namespace CommunityHub.Application.Modules.Buildings.Database.Repositories.Ads
{
    public class AdMatchSubscriptionRepository: IAdMatchSubscriptionRepository
    {
        public void Create(AdMatchSubscription subscription) {
            PostgresQuery.Create(
                @"INSERT INTO ad_match_subscriptions
                (ad_id)
                VALUES
                (@ad_id)"    
            )
            .AddParameter("ad_id", subscription.SubscribedAdId)
            .Execute();
        }

        public void DeleteByAdId(long adId)
        {
            PostgresQuery.Create(
                @"DELETE FROM ad_match_subscriptions
                WHERE ad_id = @ad_id"
            )
            .AddParameter("ad_id", adId)
            .Execute();
        }

        public bool IsSubscribed(long adId)
        {
            var query = PostgresQuery.Create(
                @"SELECT EXISTS (SELECT 1 FROM ad_match_subscriptions WHERE ad_id = @ad_id)"
            ).AddParameter("ad_id", adId);
            return query.ExecuteScalar<bool>(value => Convert.ToBoolean(value));
        }
    }
}
