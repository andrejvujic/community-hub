using CommunityHub.Application.Database;
using CommunityHub.Application.Database.Mapping.EntityMappers.Notifications.Details;
using CommunityHub.Application.Domain.Notifications.Details;
using CommunityHub.Application.Domain.Notifications.RepositoryInterfaces;

namespace CommunityHub.Application.Database.Repositories.Shared.Notifications
{
    public class NotificationDetailsRepository : INotificationDetailsRepository
    {
        private readonly AdEventNotificationDetailsMapper _adEventMapper = new();

        public void Create(NotificationDetails details)
        {
            using var connection = DatabaseConnection.Create();

            switch (details)
            {
                case AdEventNotificationDetails:
                    CreateAdEventNotificationDetails(
                        connection,
                        (AdEventNotificationDetails)details
                    );
                    break;
            }
        }

        public void CreateMany(List<NotificationDetails> details)
        {
            using var connection = DatabaseConnection.Create();

            foreach (var d in details)
            {
                switch (d)
                {
                    case AdEventNotificationDetails:
                        CreateAdEventNotificationDetails(
                            connection,
                            (AdEventNotificationDetails)d
                        );
                        break;
                }
            }
        }

        private void CreateAdEventNotificationDetails(
            DatabaseConnection connection,
            AdEventNotificationDetails details
        )
        {
            connection.Execute(
                DatabaseQuery.Create(
                    @"INSERT INTO ad_event_notification_details
                        (notification_id,
                        ad_id,
                        ad_event_type)
                    VALUES
                        (@details_notification_id,
                        @details_ad_id,
                        @details_ad_event_type)"
                )
                .ExtendParameters(
                    _adEventMapper.MapToQueryParameters(details)
                )
            );
        }
    }
}
