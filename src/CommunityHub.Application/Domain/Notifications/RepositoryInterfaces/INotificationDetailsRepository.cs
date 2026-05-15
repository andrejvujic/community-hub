using CommunityHub.Application.Database.Mapping.EntityMappers.Notifications.Details;
using CommunityHub.Application.Domain.Notifications.Details;

namespace CommunityHub.Application.Domain.Notifications.RepositoryInterfaces
{
    public interface INotificationDetailsRepository
    {
        public void Create(NotificationDetails details);
        public void CreateMany(List<NotificationDetails> details);
    }
}
