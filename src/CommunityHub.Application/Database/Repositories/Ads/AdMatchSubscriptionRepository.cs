using CommunityHub.Application.Domain.NoticeBoard;

namespace CommunityHub.Application.Database.Repositories.Ads
{
    public class AdMatchSubscriptionRepository
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

        public bool IsSubscribed(long adId)
        {
            var query = PostgresQuery.Create(
                @"SELECT EXISTS (SELECT 1 FROM ad_match_subscriptions WHERE ad_id = @ad_id)"
            ).AddParameter("ad_id", adId);
            return query.ExecuteScalar<bool>(value => Convert.ToBoolean(value));
        }
    }
}
