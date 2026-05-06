using CommunityHub.Application.Database;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.RepositoryInterfaces;
using System.Diagnostics;

namespace CommunityHub.Application.Modules.Buildings.Database.Repositories.Ads
{
    public class AdMatchSubscriptionRepository: IAdMatchSubscriptionRepository
    {
        public void Create(AdMatchSubscription subscription) {
            using var connection = DatabaseConnection.Create();

            connection.Execute(
                DatabaseQuery.Create(
                    @"INSERT INTO ad_match_subscriptions
                    (ad_id)
                    VALUES
                    (@ad_id)"
                )
                .AddParameter("ad_id", subscription.SubscribedAdId)
            );
        }

        public void DeleteByAdId(long adId)
        {
            using var connection = DatabaseConnection.Create();

            connection.Execute(
                DatabaseQuery.Create(
                    @"DELETE FROM ad_match_subscriptions
                    WHERE ad_id = @ad_id"
                )
                .AddParameter("ad_id", adId)
            );
        }

        public void DeleteManyByAdId(List<long> adIds)
        {
            using var connection = DatabaseConnection.Create();

            connection.BeginTransaction();

            try
            {
                foreach (var adId in adIds)
                    connection.Execute(
                        DatabaseQuery.Create("DELETE FROM ad_match_subscriptions WHERE ad_id = @ad_id")
                        .AddParameter("ad_id", adId)
                    );

                connection.CommitTransaction();
            } catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                connection.RollbackTranscation();
            }
        }

        public bool ExistsWithAdId(long adId)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                @"SELECT EXISTS(
                    SELECT 1
                    FROM ad_match_subscriptions
                    WHERE ad_id = @ad_id)"
            ).AddParameter("ad_id", adId);

            return connection.ExecuteScalar(query, Convert.ToBoolean);
        }
    }
}
