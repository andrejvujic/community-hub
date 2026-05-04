using CommunityHub.Application.Database;
using CommunityHub.Application.Database.Mapping.EntityMappers.Notifications.Details;
using CommunityHub.Application.Modules.Shared.Core.Domain.Notifications.Details;
using CommunityHub.Application.Modules.Shared.Core.Domain.Notifications.RepositoryInterfaces;

namespace CommunityHub.Application.Shared.Database.Repositories.Notifications
{
    public class NotificationDetailsRepository : INotificationDetailsRepository
    {
        private readonly AdEventNotificationDetailsMapper _adEventMapper = new();

        public void Create(NotificationDetails details)
        {
            switch (details)
            {
                case AdEventNotificationDetails:
                    CreateAdEventNotificationDetails((AdEventNotificationDetails)details);
                    break;
            }
        }

        private void CreateAdEventNotificationDetails(AdEventNotificationDetails details)
        {
            PostgresQuery.Create(
                @"INSERT INTO ad_event_notification_details
                    (notification_id,
                    ad_id,
                    ad_event_type)
                VALUES
                    (@details_notification_id,
                    @details_ad_id,
                    @details_ad_event_type)"
            )
            .ExtendParameters(_adEventMapper.MapToQueryParameters(details))
            .Execute();
        }
    }
}
