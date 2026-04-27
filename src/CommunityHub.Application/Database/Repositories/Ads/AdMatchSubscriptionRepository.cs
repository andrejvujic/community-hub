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
    }
}
